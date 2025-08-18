﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Constants.Data;
using Soenneker.Cosmos.Container.Abstract;
using Soenneker.Cosmos.Repositories.Shared.Abstract;
using Soenneker.Cosmos.Repository;
using Soenneker.Dtos.IdPartitionPair;
using Soenneker.Utils.BackgroundQueue.Abstract;
using Soenneker.Utils.Method;
using Soenneker.Utils.UserContext.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Documents.Typed;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Cosmos.Repositories.Shared;

/// <inheritdoc cref="ISharedRepository{TDocument}"/>
public abstract class SharedRepository<TDocument> : CosmosRepository<TDocument>, ISharedRepository<TDocument> where TDocument : TypedDocument
{
    protected abstract string EntityType { get; }

    protected SharedRepository(ICosmosContainerUtil cosmosContainerUtil, IConfiguration config, ILogger<SharedRepository<TDocument>> logger,
        IUserContext userContext, IBackgroundQueue backgroundQueue) : base(cosmosContainerUtil, config, logger, userContext, backgroundQueue)
    {
    }

    public override async ValueTask<List<TDocument>> GetAll(double? delayMs = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TDocument> query = await BuildQueryable(null, cancellationToken).NoSync();

        query = query.Where(c => c.EntityType == EntityType);

        return await GetItems(query, delayMs, cancellationToken).NoSync();
    }

    public override async ValueTask<List<IdPartitionPair>> GetAllIds(double? delayMs = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TDocument> query = await BuildQueryable(null, cancellationToken);

        query = query.Where(c => c.EntityType == EntityType);

        return await GetIds(query, delayMs, cancellationToken).NoSync();
    }

    public override async ValueTask DeleteAll(double? delayMs = null, bool useQueue = false, CancellationToken cancellationToken = default)
    {
        Logger.LogWarning("-- COSMOS: {method} (General.{type}Document) w/ {delayMs}ms delay between docs", MethodUtil.Get(), EntityType,
            delayMs.GetValueOrDefault());

        List<IdPartitionPair> ids = await GetAllIds(delayMs, cancellationToken).NoSync();

        await DeleteIds(ids, delayMs, useQueue, cancellationToken).NoSync();

        Logger.LogDebug("-- COSMOS: Finished {method} (General.{type}Document)", MethodUtil.Get(), EntityType);
    }

    public override async ValueTask DeleteAllPaged(int pageSize = DataConstants.DefaultCosmosPageSize, double? delayMs = null, bool useQueue = false,
        CancellationToken cancellationToken = default)
    {
        Logger.LogWarning("-- COSMOS: {method} (General.{type}) w/ {delayMs}ms delay between docs", MethodUtil.Get(), typeof(TDocument).Name,
            delayMs.GetValueOrDefault());

        IQueryable<TDocument> query = await BuildPagedQueryable(pageSize, cancellationToken: cancellationToken).NoSync();
        query = query.Where(c => c.EntityType == EntityType);
        query = query.OrderBy(c => c.CreatedAt);

        var newQuery = query.Select(c => new { c.DocumentId, c.PartitionKey });

        await ExecuteOnGetItemsPaged(newQuery, async results =>
        {
            Logger.LogDebug("Number of rows to be deleted in page: {rows}", results.Count);

            foreach (var result in results)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await DeleteItem(result.DocumentId, result.PartitionKey, useQueue, cancellationToken).NoSync();
            }
        }, cancellationToken).NoSync();

        Logger.LogDebug("-- COSMOS: Finished {method} (General.{type})", MethodUtil.Get(), typeof(TDocument).Name);
    }
}