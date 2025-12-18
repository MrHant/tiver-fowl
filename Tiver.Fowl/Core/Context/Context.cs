namespace Tiver.Fowl.Core.Context
{
    using System;
    using System.Collections.Concurrent;

    public static class Context
    {
        private static readonly IStorage SessionContext = new Storage();
        private static readonly ConcurrentDictionary<string, IStorage> TestContext = new ConcurrentDictionary<string, IStorage>();

        internal static Func<string> TestKey { get; set; }

        private static string CurrentTestKey =>
            TestKey?.Invoke() ?? throw new InvalidOperationException("Test key provider is not set. Call Context.SetTestKey(...) before accessing test context.");
        
        internal static IStorage Session
        {
            get
            {
                return SessionContext;
            }
        }

        internal static IStorage Test
        {
            get
            {
                return TestContext.GetOrAdd(CurrentTestKey, _ => new Storage());
            }
        }

        public static void SetTestKey(Func<string> testKey)
        {
            TestKey = testKey;
        }
        
        public static void ClearTestContext()
        {
            var key = CurrentTestKey;
            if (TestContext.TryRemove(key, out var storage))
            {
                storage.Clear();
            }
        }

        public static void ClearSessionContext()
        {
            Session.Clear();
        }
    }
}
