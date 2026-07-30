namespace Tests.FrameworkTests;

using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tiver.Fowl.Core.Context;
using Tiver.Fowl.TestingBase;

/// <summary>
/// Covers the ambient test scope directly. This fixture deliberately does not derive from
/// <see cref="BaseTestForNUnit"/> so it can drive scope creation itself.
/// </summary>
public class ContextTests
{
    /// <summary>A test type without [WebDriverTest], so Flow.Setup starts no browser.</summary>
    private sealed class PlainTest
    {
    }

    [Test]
    public void Test_ThrowsWhenNoScopeIsActive()
    {
        Context.ClearTestContext();

        var exception = Assert.Throws<InvalidOperationException>(
            () => _ = TestExecutionContext.TestName);
        StringAssert.Contains("No Tiver.Fowl test scope is active", exception!.Message);
    }

    [Test]
    public void Setup_StartsAScopeThatCarriesTestState()
    {
        Flow.Setup(typeof(PlainTest), "scoped-test");

        ClassicAssert.AreEqual("scoped-test", TestExecutionContext.TestName);
    }

    [Test]
    public void ClearTestContext_EndsTheScope()
    {
        Flow.Setup(typeof(PlainTest), "cleared-test");
        ClassicAssert.AreEqual("cleared-test", TestExecutionContext.TestName);

        Context.ClearTestContext();

        Assert.Throws<InvalidOperationException>(() => _ = TestExecutionContext.TestName);
    }

    [Test]
    public void Setup_ReplacesAnyPreviousScope()
    {
        Flow.Setup(typeof(PlainTest), "first");
        TestExecutionContext.TestStep = 11;

        Flow.Setup(typeof(PlainTest), "second");

        ClassicAssert.AreEqual("second", TestExecutionContext.TestName);
        ClassicAssert.AreEqual(0, TestExecutionContext.TestStep, "Scope reused storage from the previous test");
    }

    [Test]
    public void CurrentTestNameOrNull_IsNullOutsideAScope()
    {
        Context.ClearTestContext();

        ClassicAssert.IsNull(TestExecutionContext.CurrentTestNameOrNull);
    }

    [Test]
    public void CurrentTestNameOrNull_IsNullWhenScopeHasNoNameYet()
    {
        Context.ClearTestContext();
        ClassicAssert.IsNull(TestExecutionContext.CurrentTestNameOrNull);

        Flow.Setup(typeof(PlainTest), "named");
        ClassicAssert.AreEqual("named", TestExecutionContext.CurrentTestNameOrNull);
    }

    [Test]
    public void Storage_TryRead_DoesNotThrowForMissingKey()
    {
        IStorage storage = new Storage();

        ClassicAssert.IsFalse(storage.TryRead<string>("absent", out var missing));
        ClassicAssert.IsNull(missing);

        storage.Write("present", "value");
        ClassicAssert.IsTrue(storage.TryRead<string>("present", out var found));
        ClassicAssert.AreEqual("value", found);
    }
}
