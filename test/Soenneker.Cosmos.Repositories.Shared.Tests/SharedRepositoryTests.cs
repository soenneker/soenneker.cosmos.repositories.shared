using Soenneker.Tests.HostedUnit;

namespace Soenneker.Cosmos.Repositories.Shared.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class SharedRepositoryTests : HostedUnitTest
{
    public SharedRepositoryTests(Host host) : base(host)
    {

    }

    [Test]
    public void Default()
    {

    }
}
