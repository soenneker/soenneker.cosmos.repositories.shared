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
    /// <summary>
    /// Gets all.
    /// </summary>
    /// <param name="delayMs">The delay ms.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    [Pure]
    new ValueTask<List<TDocument>> GetAll(double? delayMs, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the any operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    [Pure]
    new ValueTask<bool> Any(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all ids.
    /// </summary>
    /// <param name="delayMs">The delay ms.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    [Pure]
    new ValueTask<List<IdPartitionPair>> GetAllIds(double? delayMs, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all.
    /// </summary>
    /// <param name="delayMs">The delay ms.</param>
    /// <param name="useQueue">The use queue.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    new ValueTask DeleteAll(double? delayMs = null, bool useQueue = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all paged.
    /// </summary>
    /// <param name="pageSize">The page size.</param>
    /// <param name="delayMs">The delay ms.</param>
    /// <param name="useQueue">The use queue.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    new ValueTask DeleteAllPaged(int pageSize, double? delayMs, bool useQueue, CancellationToken cancellationToken = default);
}