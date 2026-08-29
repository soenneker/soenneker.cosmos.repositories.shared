[![](https://img.shields.io/nuget/v/soenneker.cosmos.repositories.shared.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cosmos.repositories.shared/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cosmos.repositories.shared/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cosmos.repositories.shared/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cosmos.repositories.shared.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cosmos.repositories.shared/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cosmos.repositories.shared/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.cosmos.repositories.shared/actions/workflows/codeql.yml)

# Soenneker.Cosmos.Repositories.Shared

A data persistence abstraction layer for Cosmos DB containers that have multiple document types.

## Install

```bash
dotnet add package Soenneker.Cosmos.Repositories.Shared
```

## Quick start

```csharp
using Soenneker.Cosmos.Repositories.Shared.Abstract;

ISharedRepository<TDocument> sharedRepository = /* resolve from DI */;
var result = await sharedRepository.GetAll(1, default);
```

Gets all.

## What you get

- `ISharedRepository<TDocument>` — A data persistence abstraction layer for Cosmos DB containers that have multiple document types.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `ISharedRepository<TDocument>.Any(cancellationToken)` | Checks whether the shared repository contains any document of this entity type. | Returns `true` when at least one document exists; otherwise, `false`. |
| `ISharedRepository<TDocument>.DeleteAllPagedParallel(maxConcurrency, pageSize, cancellationToken)` | Deletes all documents of this shared entity type page-by-page with bounded parallelism. | Completes when the requested deletion has finished. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
