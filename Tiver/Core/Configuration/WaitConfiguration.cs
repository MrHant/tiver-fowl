namespace Tiver.Core.Configuration
{
    public class WaitConfiguration : IWaitConfiguration
    {
        public int Timeout { get; private set; }

        public int PollingInterval { get; private set; }

        public WaitConfiguration(int timeout, int pollingInterval)
        {
            Timeout = timeout;
            PollingInterval = pollingInterval;
        }
    }
}
