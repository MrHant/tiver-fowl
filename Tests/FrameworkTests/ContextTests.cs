namespace Tests.FrameworkTests;

using System;
using System.Collections.Concurrent;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tiver.Fowl.Core.Context;

public class ContextTests
{
    [Test]
    public void ClearTestContext_RemovesStorageFromDictionary()
    {
        var key = $"test-{Guid.NewGuid():N}";
        Context.SetTestKey(() => key);

        _ = GetCurrentTestStorage();

        var dict = GetTestContextDictionary();
        ClassicAssert.IsTrue(dict.ContainsKey(key));

        Context.ClearTestContext();

        ClassicAssert.IsFalse(dict.ContainsKey(key));
    }

    private static object GetCurrentTestStorage()
    {
        var prop = typeof(Context).GetProperty("Test", BindingFlags.NonPublic | BindingFlags.Static);
        ClassicAssert.IsNotNull(prop);
        return prop!.GetValue(null)!;
    }

    private static ConcurrentDictionary<string, IStorage> GetTestContextDictionary()
    {
        var field = typeof(Context).GetField("TestContext", BindingFlags.NonPublic | BindingFlags.Static);
        ClassicAssert.IsNotNull(field);
        return (ConcurrentDictionary<string, IStorage>)field!.GetValue(null)!;
    }
}
