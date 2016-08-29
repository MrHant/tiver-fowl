namespace Tiver.Fowl.Core.Configuration
{
    public interface IWaitConfiguration
    {
        int Timeout { get; }

        int PollingInterval { get; }
    }
}
