namespace Tiver.Core.Context
{
    public static class GeneralContext
    {
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
