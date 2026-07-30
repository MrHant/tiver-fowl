namespace Tiver.Fowl.Core.Context
{
    /// <summary>
    /// Ambient per-test scope holding that test's isolated storage.
    /// A reference type on purpose: <see cref="Context"/> publishes it through an
    /// <see cref="System.Threading.AsyncLocal{T}"/>, so writes to <see cref="Storage"/> made after
    /// the scope was installed are visible everywhere the scope flowed, without further
    /// execution-context mutation.
    /// </summary>
    internal sealed class TestScope
    {
        public IStorage Storage { get; } = new Storage();
    }
}
