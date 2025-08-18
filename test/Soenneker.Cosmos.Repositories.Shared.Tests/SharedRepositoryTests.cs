using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Cosmos.Repositories.Shared.Tests;

[Collection("Collection")]
public sealed class SharedRepositoryTests : FixturedUnitTest
{
    public SharedRepositoryTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {

    }

    [Fact]
    public void Default()
    {

    }
}
