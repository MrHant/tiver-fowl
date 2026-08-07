namespace Tiver.Fowl.Core.Context
{
    using System;
    using System.Threading;

    /// <summary>
    /// Storage for the two levels a test run has: the session, and the running test.
    /// </summary>
    /// <remarks>
    /// Both levels are a <see cref="StorageScope"/> holding a public store for the test author and
    /// an internal one for the framework's own state. They differ only in lifetime and publication:
    /// the session scope is a single instance living as long as the process, while the test scope is
    /// published through an <see cref="AsyncLocal{T}"/> so it follows its own test across thread
    /// hops and <c>await</c>s and never reaches a sibling test that reuses the same worker thread.
    /// That difference is also why the session accessors cannot fail and the test ones can — outside
    /// a test there is no test scope to read.
    /// </remarks>
    public static class Context
    {
        private static readonly StorageScope SessionScope = new();

        private static readonly AsyncLocal<StorageScope?> TestScope = new();

        private const string NoScopeMessage =
            "No Tiver.Fowl test scope is active. Flow.Setup(...) must be called from the test's " +
            "synchronous setup method before any test context is accessed. Installing the scope " +
            "from an async setup method does not work.";

        /// <summary>
        /// Per-test scratch space for test authors: values written here are visible only to the
        /// running test and are discarded when it ends. This is the supported place for state a
        /// test needs to carry between steps — a created record's ID, a generated username — which
        /// must not live in a static field if the suite runs in parallel.
        /// </summary>
        /// <example>
        /// <code>
        /// Context.TestStorage.Write("userId", createdId);
        /// var userId = Context.TestStorage.Read&lt;string&gt;("userId");
        /// </code>
        /// </example>
        /// <exception cref="InvalidOperationException">
        /// No test scope is active — see <see cref="TestingBase.Flow.Setup"/>.
        /// </exception>
        public static IStorage TestStorage => CurrentTestScope.UserStorage;

        /// <summary>
        /// Scratch space shared by every test in the run. Unlike <see cref="TestStorage"/> this is
        /// reachable outside a test, so it also serves session setup and teardown. It is shared
        /// mutable state across parallel tests: treat it as read-mostly, and prefer
        /// <see cref="TestStorage"/> for anything a single test owns.
        /// </summary>
        public static IStorage SessionStorage => SessionScope.UserStorage;

        /// <summary>
        /// The framework's own session state, kept apart from the consumer-facing
        /// <see cref="SessionStorage"/> so the two cannot collide.
        /// </summary>
        internal static IStorage Session => SessionScope.FrameworkStorage;

        /// <summary>
        /// The framework's own per-test state, kept apart from the consumer-facing
        /// <see cref="TestStorage"/> so the two cannot collide.
        /// </summary>
        internal static IStorage Test => CurrentTestScope.FrameworkStorage;

        /// <summary>
        /// Non-throwing view of the current framework test storage, for ambient consumers that
        /// legitimately run outside a test — Serilog enrichers, session setup, report generation.
        /// </summary>
        internal static IStorage? TestOrNull => TestScope.Value?.FrameworkStorage;

        private static StorageScope CurrentTestScope =>
            TestScope.Value ?? throw new InvalidOperationException(NoScopeMessage);

        /// <summary>
        /// Starts a fresh scope for the calling test. Must be called from a synchronous method.
        /// </summary>
        internal static void BeginTestScope()
        {
            TestScope.Value = new StorageScope();
        }

        public static void ClearTestContext()
        {
            TestScope.Value?.Clear();
            TestScope.Value = null;
        }

        public static void ClearSessionContext()
        {
            SessionScope.Clear();
        }
    }
}
