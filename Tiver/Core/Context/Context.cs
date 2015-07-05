namespace Tiver.Core.Context
{
    internal static class Context
    {
        private static IStorage session = new Storage();
        private static IStorage test = new Storage();

        internal static IStorage Session
        {
            get
            {
                return session;
            }
        }

        internal static IStorage Test
        {
            get
            {
                return test;
            }
        }
    }
}
