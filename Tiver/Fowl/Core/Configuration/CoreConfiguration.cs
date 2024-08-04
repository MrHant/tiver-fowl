using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiver.Fowl.Core.Configuration
{
    public class ApplicationConfigurationOptions
    {
        public string Title { get; set; }
        public string StartUrl {  get; set; }
    }

    public class CoreConfiguration
    {
        public ApplicationConfigurationOptions Application { get; set; }
    }
}
