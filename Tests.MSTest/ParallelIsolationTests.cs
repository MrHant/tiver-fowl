using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tiver.Fowl.Core.Context;
using Tiver.Fowl.TestingBase;

namespace Tests.MSTest;

/// <summary>
/// Every test sees its own test context under MSTest method-level parallelism, including after an
/// await and inside spawned work.
///
/// The rendezvous holds each test just after setup so their lifecycles genuinely overlap; without
/// it they can finish quickly enough to pass even when the context is shared between them.
/// </summary>
[TestClass]
public class ParallelIsolationTests : BaseTestForMSTest
{
    private static int _setupsCompleted;

    /// <summary>
    /// Blocks until some other test has completed its setup after this one did. A context shared
    /// between tests is one that a later Flow.Setup can overwrite, so the assertions that follow
    /// run at the point where any bleed between tests becomes observable. Waiting for a successor
    /// rather than for a fixed number of participants keeps that guarantee when the runner
    /// allocates fewer workers than there are tests.
    ///
    /// The last test to run has no successor and times out; by then every other setup has already
    /// happened, so it is covered anyway.
    /// </summary>
    private static void WaitForALaterSetup()
    {
        var mine = Interlocked.Increment(ref _setupsCompleted);
        var deadline = DateTime.UtcNow.AddSeconds(5);
        while (Volatile.Read(ref _setupsCompleted) <= mine && DateTime.UtcNow < deadline)
        {
            Thread.Sleep(10);
        }
    }

    private string ExpectedTestName
    {
        get
        {
            var testType = GetType();
            return $"{testType.Namespace}.{testType.Name}.{TestContext.TestName}";
        }
    }

    private void AssertOwnContext(int step)
    {
        Assert.AreEqual(ExpectedTestName, TestExecutionContext.TestName, "Test observed another test's context");
        Assert.AreEqual(step, TestExecutionContext.TestStep, "Test observed another test's storage");
    }

    [TestMethod] public void Isolated1() => RunSync(1);

    [TestMethod] public void Isolated2() => RunSync(2);

    [TestMethod] public void Isolated3() => RunSync(3);

    [TestMethod] public void Isolated4() => RunSync(4);

    [TestMethod]
    public async Task IsolatedAcrossAwait()
    {
        TestExecutionContext.TestStep = 5;
        WaitForALaterSetup();

        // Resumes on an arbitrary pool thread, quite possibly one owned by another test.
        await Task.Delay(50).ConfigureAwait(false);

        AssertOwnContext(5);
    }

    [TestMethod]
    public async Task IsolatedInsideSpawnedTask()
    {
        TestExecutionContext.TestStep = 6;
        WaitForALaterSetup();

        await Task.Run(() => AssertOwnContext(6)).ConfigureAwait(false);
    }

    private void RunSync(int step)
    {
        TestExecutionContext.TestStep = step;
        WaitForALaterSetup();
        AssertOwnContext(step);
    }
}
