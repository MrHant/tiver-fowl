namespace Tests.Configuration
{
    using System;
    using System.Collections.Generic;

    public class UrlsConfiguration
    {
        public Dictionary<string, string> Urls { get; set; } = new();

        public Uri GetUrl(string name)
        {
            if (!Urls.TryGetValue(name, out var url))
            {
                throw new KeyNotFoundException(
                    $"URL with name '{name}' not found in configuration. " +
                    $"Available URLs: {string.Join(", ", Urls.Keys)}");
            }

            return new Uri(url);
        }

        public bool TryGetUrl(string name, out Uri url)
        {
            url = null;
            if (Urls.TryGetValue(name, out var urlString))
            {
                url = new Uri(urlString);
                return true;
            }

            return false;
        }
    }
}
