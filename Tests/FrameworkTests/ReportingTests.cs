namespace Tests.FrameworkTests
{
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Reporting;
    using Tiver.Fowl.TestingBase;

    /// <summary>
    /// Tests to verify the reporting functionality without requiring a browser.
    /// These tests are marked as [Explicit] and won't run during normal test execution.
    /// Run them explicitly with: dotnet test --filter "FullyQualifiedName~ReportingTests"
    /// </summary>
    [Explicit("Framework tests for report generation - excluded from regular test runs")]
    public class ReportingTests : BaseTestForNUnit
    {
        [Test]
        public void PassingTest()
        {
            this.LogStep("First step of passing test");
            this.LogStep("Second step of passing test");
            ClassicAssert.IsTrue(true);
        }

        [Test]
        public void FailingTest()
        {
            this.LogStep("Step before failure");
            ClassicAssert.Fail("Intentional failure for report testing");
        }

        [Test]
        public void AnotherPassingTest()
        {
            this.LogStep("Only step in this test");
            ClassicAssert.AreEqual(42, 42);
        }
    }

    /// <summary>
    /// Unit tests for LogFileParser - these don't require the base test class
    /// </summary>
    [TestFixture]
    public class LogFileParserTests
    {
        [Test]
        public void Parse_GroupsActionsUnderSteps()
        {
            // Create a sample log file with steps and element actions
            var logContent = @"
{""Timestamp"":""2025-12-18T10:00:00.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""Step #{Step} :: {Text}"",""Properties"":{""Step"":1,""Text"":""First step"",""LogType"":""TestStep"",""TestName"":""TestWithActions""}}
{""Timestamp"":""2025-12-18T10:00:01.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""{Type} '{Name}' - {Action}"",""Properties"":{""Type"":""Button"",""Name"":""Submit"",""Action"":""Click"",""LogType"":""ElementAction"",""TestName"":""TestWithActions""}}
{""Timestamp"":""2025-12-18T10:00:02.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""{Type} '{Name}' - {Action}"",""Properties"":{""Type"":""Textbox"",""Name"":""Username"",""Action"":""Type"",""LogType"":""ElementAction"",""TestName"":""TestWithActions""}}
{""Timestamp"":""2025-12-18T10:00:03.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""Step #{Step} :: {Text}"",""Properties"":{""Step"":2,""Text"":""Second step"",""LogType"":""TestStep"",""TestName"":""TestWithActions""}}
{""Timestamp"":""2025-12-18T10:00:04.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""{Type} '{Name}' - {Action}"",""Properties"":{""Type"":""Link"",""Name"":""Home"",""Action"":""Click"",""LogType"":""ElementAction"",""TestName"":""TestWithActions""}}
{""Timestamp"":""2025-12-18T10:00:05.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""Test result - '{TestResult}'"",""Properties"":{""TestResult"":""Passed"",""LogType"":""TestResult"",""TestName"":""TestWithActions""}}
".Trim();

            var tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, logContent);

                var sessions = LogFileParser.Parse(tempFile);
                var results = sessions.Values.SelectMany(r => r).ToList();

                ClassicAssert.AreEqual(1, results.Count);
                var test = results[0];

                ClassicAssert.AreEqual("TestWithActions", test.TestName);
                ClassicAssert.AreEqual(2, test.Steps.Count);

                // First step should have 2 actions (Button click and Textbox type)
                var step1 = test.Steps[0];
                ClassicAssert.AreEqual(1, step1.StepNumber);
                ClassicAssert.AreEqual("First step", step1.Description);
                ClassicAssert.AreEqual(2, step1.Actions.Count);
                ClassicAssert.AreEqual("Button", step1.Actions[0].ElementType);
                ClassicAssert.AreEqual("Submit", step1.Actions[0].ElementName);
                ClassicAssert.AreEqual("Textbox", step1.Actions[1].ElementType);
                ClassicAssert.AreEqual("Username", step1.Actions[1].ElementName);

                // Second step should have 1 action (Link click)
                var step2 = test.Steps[1];
                ClassicAssert.AreEqual(2, step2.StepNumber);
                ClassicAssert.AreEqual("Second step", step2.Description);
                ClassicAssert.AreEqual(1, step2.Actions.Count);
                ClassicAssert.AreEqual("Link", step2.Actions[0].ElementType);
                ClassicAssert.AreEqual("Home", step2.Actions[0].ElementName);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Test]
        public void Parse_PreStepActionsGroupedSeparately()
        {
            // Actions that occur before any step should be in PreStepActions
            var logContent = @"
{""Timestamp"":""2025-12-18T10:00:00.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""{Type} '{Name}' - {Action}"",""Properties"":{""Type"":""Button"",""Name"":""InitialButton"",""Action"":""Click"",""LogType"":""ElementAction"",""TestName"":""TestWithPreStepActions""}}
{""Timestamp"":""2025-12-18T10:00:01.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""Step #{Step} :: {Text}"",""Properties"":{""Step"":1,""Text"":""First step"",""LogType"":""TestStep"",""TestName"":""TestWithPreStepActions""}}
{""Timestamp"":""2025-12-18T10:00:02.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""{Type} '{Name}' - {Action}"",""Properties"":{""Type"":""Textbox"",""Name"":""Username"",""Action"":""Type"",""LogType"":""ElementAction"",""TestName"":""TestWithPreStepActions""}}
{""Timestamp"":""2025-12-18T10:00:03.0000000+00:00"",""Level"":""Information"",""MessageTemplate"":""Test result - '{TestResult}'"",""Properties"":{""TestResult"":""Passed"",""LogType"":""TestResult"",""TestName"":""TestWithPreStepActions""}}
".Trim();

            var tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, logContent);

                var sessions = LogFileParser.Parse(tempFile);
                var results = sessions.Values.SelectMany(r => r).ToList();

                ClassicAssert.AreEqual(1, results.Count);
                var test = results[0];

                // Pre-step action should be in PreStepActions
                ClassicAssert.AreEqual(1, test.PreStepActions.Count);
                ClassicAssert.AreEqual("Button", test.PreStepActions[0].ElementType);
                ClassicAssert.AreEqual("InitialButton", test.PreStepActions[0].ElementName);

                // Step action should be under the step
                ClassicAssert.AreEqual(1, test.Steps.Count);
                ClassicAssert.AreEqual(1, test.Steps[0].Actions.Count);
                ClassicAssert.AreEqual("Textbox", test.Steps[0].Actions[0].ElementType);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
