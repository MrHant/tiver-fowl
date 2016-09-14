namespace $rootnamespace$
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Fowl.TestingBase;

    [TestClass]
    public class AssemblyCleanup : BaseTestForMSTest
    {
        [AssemblyCleanupAttribute]
        public static void Cleanup()
        {
            AssemblyTeardown();
        }
    }
}
