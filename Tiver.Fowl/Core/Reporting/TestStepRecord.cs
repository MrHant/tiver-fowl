namespace Tiver.Fowl.Core.Reporting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Record of a single test step logged via LogStep(), including element actions that occurred after it
    /// </summary>
    public class TestStepRecord
    {
        public int StepNumber { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        private readonly List<ElementActionRecord> _actions = new();

        /// <summary>
        /// Element actions that occurred after this step (until the next step)
        /// </summary>
        public IReadOnlyList<ElementActionRecord> Actions => _actions;

        /// <summary>
        /// Add an element action to this step
        /// </summary>
        public void AddAction(ElementActionRecord action)
        {
            _actions.Add(action);
        }
    }
}
