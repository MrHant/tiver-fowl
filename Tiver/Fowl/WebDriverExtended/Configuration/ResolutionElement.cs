namespace Tiver.Fowl.WebDriverExtended.Configuration
{
    using System.Configuration;
    using Contracts.Configuration;

    public class ResolutionElement : IResolution
    {
        public int? Width { get; set; } = null;

        public int? Height { get; set; } = null;
    }
}
