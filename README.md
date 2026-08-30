[![](https://img.shields.io/nuget/v/soenneker.cosmos.repositories.shared.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cosmos.repositories.shared/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cosmos.repositories.shared/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cosmos.repositories.shared/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cosmos.repositories.shared.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cosmos.repositories.shared/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cosmos.repositories.shared/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.cosmos.repositories.shared/actions/workflows/codeql.yml)

# Soenneker.Cosmos.Repositories.Shared

An abstract Cosmos repository base for isolating one typed-document model inside a container shared by several models.

## Installation

```bash
dotnet add package Soenneker.Cosmos.Repositories.Shared
```

## Define a repository

Derive from `SharedRepository<TDocument>`, where the document extends `TypedDocument`. Each derived repository must choose both its container and the `EntityType` discriminator stored on its documents.

```csharp
public interface IProductRepository : ISharedRepository<ProductDocument>
{
}

public sealed class ProductRepository : SharedRepository<ProductDocument>, IProductRepository
{
    public override string ContainerName => "catalog";
    protected override string EntityType => "product";

    public ProductRepository(
        ICosmosContainerUtil containerUtil,
        IConfiguration configuration,
        ILogger<SharedRepository<ProductDocument>> logger,
        IUserContext userContext,
        IBackgroundQueue backgroundQueue,
        IMemoryStreamUtil memoryStreamUtil)
        : base(containerUtil, configuration, logger, userContext, backgroundQueue, memoryStreamUtil)
    {
    }
}
```

Register the concrete repository as scoped because it consumes scoped user context:

```csharp
services.AddScoped<IProductRepository, ProductRepository>();
```

This package has no registrar. Register the Cosmos container, user context, background queue, memory-stream utility, configuration, and logging dependencies required by the base constructor.

## Discriminator-scoped operations

The following operations automatically add an `EntityType` filter:

- `GetAll` and `GetAllIds`
- `Any`, `None`, and `Count`
- `DeleteAll`, `DeleteAllPaged`, and `DeleteAllPagedParallel`

Other inherited methods follow the `ICosmosRepository<TDocument>` contract and use the IDs, partition keys, or queries supplied by the caller. They do not automatically prevent access to another discriminator. Use unique, stable discriminator values and avoid exposing an unrestricted repository to untrusted callers.

`DeleteAll` first loads all matching IDs. `DeleteAllPaged` processes ordered pages sequentially and can queue deletes. `DeleteAllPagedParallel` performs direct deletes with bounded concurrency and requires `maxConcurrency` of at least `1`.

Bulk deletion is permanent and not transactional across the result set. A failure or cancellation propagates after already completed or queued work; it does not roll that work back.
