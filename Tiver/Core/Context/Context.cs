namespace Tiver.Core.Context
{
    public static class Context
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

        public static void ClearTestContext()
        {
            Context.Test.Clear();
        }

        public static void ClearSessionContext()
        {
            Context.Session.Clear();
        }
    }
}
