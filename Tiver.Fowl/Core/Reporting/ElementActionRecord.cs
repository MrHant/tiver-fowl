namespace Tiver.Fowl.Core.Reporting
{
    using System;

    /// <summary>
    /// Record of a single element interaction (click, type, etc.)
    /// </summary>
    public class ElementActionRecord
    {
        public string ElementType { get; set; }

        public string ElementName { get; set; }

        public string Action { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
