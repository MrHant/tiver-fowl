namespace Tiver.Fowl.Core.Reporting
{
    using System;

    /// <summary>
    /// Record of a single element interaction (click, type, etc.)
    /// </summary>
    public class ElementActionRecord
    {
        public string ElementType { get; set; } = string.Empty;

        public string ElementName { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}
