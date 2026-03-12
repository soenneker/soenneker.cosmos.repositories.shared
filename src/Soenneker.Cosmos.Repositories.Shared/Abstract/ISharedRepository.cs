using Soenneker.Cosmos.Repository.Abstract;
using Soenneker.Dtos.IdPartitionPair;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cosmos.Repositories.Shared.Abstract;

/// <summary>
/// A data persistence abstraction layer for Cosmos DB containers that have multiple document types
/// </summary>
public interface ISharedRepository<TDocument> : ICosmosRepository<TDocument> where TDocument : class
{
    [Pure]
    new ValueTask<List<TDocument>> GetAll(double? delayMs, CancellationToken cancellationToken = default);

    [Pure]
    new ValueTask<bool> Any(CancellationToken cancellationToken = default);

    [Pure]
    new ValueTask<List<IdPartitionPair>> GetAllIds(double? delayMs, CancellationToken cancellationToken = default);

    new ValueTask DeleteAll(double? delayMs = null, bool useQueue = false, CancellationToken cancellationToken = default);

    new ValueTask DeleteAllPaged(int pageSize, double? delayMs, bool useQueue, CancellationToken cancellationToken = default);
}