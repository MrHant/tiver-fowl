namespace Tiver.Fowl.Core.Context
{
    using System;
    using System.Threading;

    public static class Context
    {
        private static readonly IStorage SessionContext = new Storage();

        /// <summary>
        /// The running test's scope. <see cref="AsyncLocal{T}"/> flows into async continuations and
        /// spawned tasks, so a scope follows its own test across thread hops, and each test begins
        /// from a clean execution context — a scope never reaches a sibling test that reuses the
        /// same worker thread.
        /// </summary>
        private static readonly AsyncLocal<TestScope> CurrentScope = new();

        internal static IStorage Session => SessionContext;

        internal static IStorage Test =>
            CurrentScope.Value?.Storage ?? throw new InvalidOperationException(
                "No Tiver.Fowl test scope is active. Flow.Setup(...) must be called from the " +
                "test's synchronous setup method before any test context is accessed. Installing " +
                "the scope from an async setup method does not work.");

        /// <summary>
        /// Non-throwing view of the current test storage, for ambient consumers that legitimately
        /// run outside a test — Serilog enrichers, session setup, report generation.
        /// </summary>
        internal static IStorage TestOrNull => CurrentScope.Value?.Storage;

        /// <summary>
        /// Starts a fresh scope for the calling test. Must be called from a synchronous method.
        /// </summary>
        internal static void BeginTestScope()
        {
            CurrentScope.Value = new TestScope();
        }

        public static void ClearTestContext()
        {
            CurrentScope.Value?.Storage.Clear();
            CurrentScope.Value = null;
        }

        public static void ClearSessionContext()
        {
            Session.Clear();
        }
    }
}
