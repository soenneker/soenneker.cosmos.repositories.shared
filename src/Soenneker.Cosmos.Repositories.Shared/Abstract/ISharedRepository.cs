using Soenneker.Cosmos.Repository.Abstract;
using Soenneker.Dtos.IdPartitionPair;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cosmos.Repositories.Shared.Abstract;

/// <summary>
/// Defines repository operations for one typed-document discriminator in a Cosmos container shared by multiple document types.
/// </summary>
public interface ISharedRepository<TDocument> : ICosmosRepository<TDocument> where TDocument : class
{
    /// <summary>
    /// Gets all.
    /// </summary>
    /// <param name="delayMs">Delay in milliseconds before the action runs.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the collection returned by get All.</returns>
    [Pure]
    new ValueTask<List<TDocument>> GetAll(double? delayMs, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether any document exists for this repository's entity type.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>true if retrieves any from the Shared Repository; otherwise, false.</returns>
    [Pure]
    new ValueTask<bool> Any(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether no documents exist for this repository's entity type.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when no matching document exists; otherwise, <see langword="false"/>.</returns>
    [Pure]
    new ValueTask<bool> None(CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts documents belonging to this repository's entity type.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The number of matching documents.</returns>
    [Pure]
    new ValueTask<int> Count(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all ids.
    /// </summary>
    /// <param name="delayMs">Delay in milliseconds before the action runs.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the collection returned by get All Ids.</returns>
    [Pure]
    new ValueTask<List<IdPartitionPair>> GetAllIds(double? delayMs, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all.
    /// </summary>
    /// <param name="delayMs">Delay in milliseconds before the action runs.</param>
    /// <param name="useQueue">Whether to enqueue the write for background execution instead of awaiting Redis directly.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes after the targeted files have been deleted.</returns>
    new ValueTask DeleteAll(double? delayMs = null, bool useQueue = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all paged.
    /// </summary>
    /// <param name="pageSize">Maximum number of items to request per page.</param>
    /// <param name="delayMs">Delay in milliseconds before the action runs.</param>
    /// <param name="useQueue">Whether to enqueue the write for background execution instead of awaiting Redis directly.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes after the targeted files have been deleted.</returns>
    new ValueTask DeleteAllPaged(int pageSize, double? delayMs, bool useQueue, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all documents of this shared entity type page-by-page with bounded parallelism.
    /// </summary>
    /// <param name="maxConcurrency">The maximum number of concurrent delete operations.</param>
    /// <param name="pageSize">Maximum number of items to request per page.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes after the targeted files have been deleted.</returns>
    new ValueTask DeleteAllPagedParallel(int maxConcurrency, int pageSize, CancellationToken cancellationToken = default);
}
