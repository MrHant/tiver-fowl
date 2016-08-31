namespace Tiver.Fowl.Core.Configuration
{
    using System;

    public interface IApplicationConfiguration
    {
        string Title { get; }

        Uri StartUrl { get; }
    }
}
