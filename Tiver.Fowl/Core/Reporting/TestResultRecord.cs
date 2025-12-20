namespace Tiver.Fowl.Core.Reporting
{
    using System;
    using System.Collections.Generic;
    using Enums;

    /// <summary>
    /// Record of a single test execution parsed from log file
    /// </summary>
    public class TestResultRecord
    {
        public string TestName { get; set; }

        public TestResult Result { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan Duration => EndTime - StartTime;

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public string ScreenshotBase64 { get; set; }

        private readonly List<TestStepRecord> _steps = new();
        private readonly List<ElementActionRecord> _preStepActions = new();

        /// <summary>
        /// Test steps logged via LogStep()
        /// </summary>
        public IReadOnlyList<TestStepRecord> Steps => _steps;

        /// <summary>
        /// Element actions that occurred before the first step
        /// </summary>
        public IReadOnlyList<ElementActionRecord> PreStepActions => _preStepActions;

        /// <summary>
        /// Add a step record
        /// </summary>
        public void AddStep(TestStepRecord step)
        {
            _steps.Add(step);
        }

        /// <summary>
        /// Add an element action that occurred before the first step
        /// </summary>
        public void AddPreStepAction(ElementActionRecord action)
        {
            _preStepActions.Add(action);
        }

        /// <summary>
        /// Get the current (last) step, or null if no steps yet
        /// </summary>
        public TestStepRecord CurrentStep => _steps.Count > 0 ? _steps[^1] : null;
    }
}
