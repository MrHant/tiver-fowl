using NUnit.Framework;
using Tiver.Fowl.Core.Context;
using Tiver.Fowl.TestingBase;

namespace Tests.NUnit;

/// <summary>
/// Per-test storage, used exactly as a consumer would. This project compiles against the packed
/// NuGet package rather than the project, so it is the thing that actually proves the API is public
/// and reachable from outside the framework — the repository's own tests would pass either way.
/// </summary>
[TestFixture]
public class TestStorageTests : BaseTestForNUnit
{
    [Test]
    public void TestStorageCarriesStateBetweenSteps()
    {
        Context.TestStorage.Write("createdUserId", "u-1001");

        Assert.That(Context.TestStorage.Read<string>("createdUserId"), Is.EqualTo("u-1001"));
    }

    [Test]
    public void TestStorageStartsEmptyForEveryTest()
    {
        Assert.That(Context.TestStorage.TryRead<string>("createdUserId", out _), Is.False);
    }

    [Test]
    public void SessionStorageIsSharedAcrossTests()
    {
        Context.SessionStorage.Write("suiteName", "consumer-suite");

        Assert.That(Context.SessionStorage.Read<string>("suiteName"), Is.EqualTo("consumer-suite"));
    }
}
