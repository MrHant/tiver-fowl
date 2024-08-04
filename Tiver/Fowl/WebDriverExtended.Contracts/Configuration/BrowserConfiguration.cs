using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiver.Fowl.WebDriverExtended.Contracts.Configuration
{
    public class ResolutionConfigurationOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class BrowserConfiguration
    {
        public string BrowserType {  get; set; }
        public bool DownloadBinary { get; set; } = false;

        public string RemoteAddress {  get; set; }

        public ResolutionConfigurationOptions Resolution {  get; set; }

    }
}
