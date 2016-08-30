namespace Tiver.Fowl.Core
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Configuration;
    using Exceptions;

    public static class Wait
    {
        public static TResult Until<TResult>(Func<TResult> condition)
        {
            WaitConfigurationSection config = (WaitConfigurationSection)System.Configuration.ConfigurationManager.GetSection("waitConfigurationGroup/waitConfiguration");
            return Until(condition, config);
        }

        public static TResult Until<TResult>(Func<TResult> condition, IWaitConfiguration configuration)
        {
            return Until(condition, configuration.Timeout, configuration.PollingInterval);
        }

        private static TResult Until<TResult>(Func<TResult> condition, int timeout, int pollingInterval)
        {
            // Start continious checking
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                var result = condition.Invoke();

                // Exit condition - some non-default result
                if (!result.Equals(default(TResult)))
                {
                    return result;
                }

                // Exit condition - timeout is reached
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                if (elapsedMilliseconds > timeout)
                {
                    stopwatch.Stop();
                    throw new WaitTimeoutException(string.Format("Wait timeout reached after {0} milliseconds waiting.", elapsedMilliseconds));
                }

                // No exit conditions met - Sleep for polling interval
                Thread.Sleep(pollingInterval);
            }
        }
    }
}
