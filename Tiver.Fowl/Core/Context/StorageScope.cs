namespace Tiver.Fowl.Core.Context
{
    /// <summary>
    /// A pair of storages sharing one lifetime. <see cref="Context"/> keeps two of these — one for
    /// the session, one for the running test — so the two levels differ only in how long they live
    /// and how they are published, not in what they hold or how they are cleared.
    /// </summary>
    /// <remarks>
    /// A reference type on purpose: the per-test instance is published through an
    /// <see cref="System.Threading.AsyncLocal{T}"/>, so writes made after the scope was installed
    /// are visible everywhere the scope flowed, without further execution-context mutation.
    /// </remarks>
    internal sealed class StorageScope
    {
        /// <summary>
        /// The framework's own state — browser, test name, result, current step, session id.
        /// </summary>
        public IStorage FrameworkStorage { get; } = new Storage();

        /// <summary>
        /// Scratch space for the test author, kept separate from <see cref="FrameworkStorage"/> so
        /// the two cannot collide. Sharing one store would mean a test writing a key named
        /// <c>"Browser"</c>, or calling <see cref="IStorage.Clear"/>, could break teardown in a way
        /// that looks nothing like its cause.
        /// </summary>
        public IStorage UserStorage { get; } = new Storage();

        /// <summary>
        /// Empties both storages. Clearing through the scope rather than at each call site means a
        /// storage added here cannot be forgotten by one of them.
        /// </summary>
        public void Clear()
        {
            FrameworkStorage.Clear();
            UserStorage.Clear();
        }
    }
}
