namespace TodoMvcEmberJsTests
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.TestingBase;
    using views;

    [TestClass]
    [WebDriverTest]
    public class SmokeTest : BaseTestForMSTest
    {
        [TestMethod]
        public void AddTaskTest()
        {
            MainPage.NewTaskField.Type("Join me");
            MainPage.NewTaskField.PressEnter();
            Thread.Sleep(TimeSpan.FromSeconds(15));
            // TODO: Add assertions
        }
    }
}
