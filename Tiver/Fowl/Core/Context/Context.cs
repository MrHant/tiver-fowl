namespace Tiver.Fowl.Core.Context
{
    using System;
    using System.Collections.Concurrent;

    public static class Context
    {
        private static readonly IStorage SessionContext = new Storage();
        private static readonly ConcurrentDictionary<string, IStorage> TestContext = new ConcurrentDictionary<string, IStorage>();

        internal static Func<string> TestKey { get; set; }
        
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
                return TestContext.GetOrAdd(TestKey.Invoke(), new Storage());
            }
        }

        public static void SetTestKey(Func<string> testKey)
        {
            TestKey = testKey;
        }
        
        public static void ClearTestContext()
        {
            Test.Clear();
        }

        public static void ClearSessionContext()
        {
            Session.Clear();
        }
    }
}
