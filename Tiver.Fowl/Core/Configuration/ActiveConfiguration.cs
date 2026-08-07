namespace Tiver.Fowl.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Tiver.Fowl.Core.Context;

    /// <summary>
    /// Provides unified access to application configuration with support for
    /// environment-based layering and nested values.
    /// </summary>
    /// <remarks>
    /// Configuration files are loaded in order (later overrides earlier):
    /// 1. config.json - base configuration
    /// 2. config.{environment}.json - environment-specific overrides
    ///
    /// Environment can be set via (in order of priority):
    /// 1. SetEnvironment() method (highest priority)
    /// 2. TIVER_ENVIRONMENT environment variable
    /// 3. "Environment" key in Tiver_config.json (lowest priority)
    /// </remarks>
    public static class ActiveConfiguration
    {
        private const string EnvironmentVariableName = "TIVER_ENVIRONMENT";
        private const string FrameworkConfigFileName = "Tiver_config.json";
        private const string BaseConfigFileName = "config.json";

        private static IConfigurationRoot _config;
        private static string? _environment;
        private static readonly object _lock = new();

        static ActiveConfiguration()
        {
            _environment = ResolveEnvironment();
            _config = BuildConfiguration();
        }

        private static string? ResolveEnvironment()
        {
            // Priority 1: Environment variable (can be overridden by SetEnvironment)
            var envVar = System.Environment.GetEnvironmentVariable(EnvironmentVariableName);
            if (!string.IsNullOrEmpty(envVar))
            {
                return envVar;
            }

            // Priority 2: Tiver_config.json
            var frameworkConfig = new ConfigurationBuilder()
                .AddJsonFile(FrameworkConfigFileName, optional: true, reloadOnChange: false)
                .Build();

            return frameworkConfig.GetValue<string>("Environment");
        }

        /// <summary>
        /// Gets the current environment name, or <c>null</c> when no environment is configured, in
        /// which case only the base <c>config.json</c> layer is loaded.
        /// </summary>
        public static string? Environment
        {
            get
            {
                lock (_lock)
                {
                    return _environment;
                }
            }
        }

        /// <summary>
        /// Sets the environment and reloads configuration.
        /// </summary>
        /// <param name="environment">Environment name (e.g., "dev", "qa", "prod"). Null or empty
        /// clears the environment layer, leaving only the base <c>config.json</c>.</param>
        public static void SetEnvironment(string? environment)
        {
            lock (_lock)
            {
                _environment = environment;
                _config = BuildConfiguration();
            }
        }

        /// <summary>
        /// Reloads configuration from files.
        /// </summary>
        public static void Reload()
        {
            lock (_lock)
            {
                _config = BuildConfiguration();
            }
        }

        /// <summary>
        /// Gets a configuration value by path.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="path">Configuration path using colon separator (e.g., "Database:Connection:Timeout")</param>
        /// <param name="defaultValue">Default value used only when the path carries no value.</param>
        /// <returns>The configured value — including a falsy one such as <c>false</c>, <c>0</c> or
        /// <c>""</c> — or <paramref name="defaultValue"/> when the path carries no value, which is
        /// null for reference types unless a default is supplied.</returns>
        /// <remarks>
        /// A path "carries no value" when it is absent, when it is explicitly null in the config
        /// file, or when it names a parent section rather than a leaf. Those are the same three
        /// cases <see cref="ConfigurationBinder.GetValue{T}(IConfiguration, string)"/> has nothing
        /// to convert for, so the default applies exactly when there is no configured value to
        /// return — and never merely because the configured value equals <c>default(T)</c>.
        /// </remarks>
        public static T? Get<T>(string path, T? defaultValue = default)
        {
            lock (_lock)
            {
                var section = _config.GetSection(path);
                return section.Value is null ? defaultValue : _config.GetValue<T>(path);
            }
        }

        /// <summary>
        /// Gets a configuration section bound to an object.
        /// </summary>
        /// <typeparam name="T">The type to bind the section to.</typeparam>
        /// <param name="path">Configuration path to the section.</param>
        /// <returns>A new instance of T with values from the section.</returns>
        public static T GetSection<T>(string path) where T : new()
        {
            lock (_lock)
            {
                var instance = new T();
                _config.GetSection(path).Bind(instance);
                return instance;
            }
        }

        /// <summary>
        /// Gets a configuration section as a dictionary.
        /// </summary>
        /// <param name="path">Configuration path to the section.</param>
        /// <returns>Dictionary of key-value pairs from the section.</returns>
        public static Dictionary<string, string> GetSectionAsDictionary(string path)
        {
            lock (_lock)
            {
                var section = _config.GetSection(path);
                var dict = new Dictionary<string, string>();
                foreach (var child in section.GetChildren())
                {
                    if (child.Value != null)
                    {
                        dict[child.Key] = child.Value;
                    }
                }
                return dict;
            }
        }

        /// <summary>
        /// Checks if a configuration path exists.
        /// </summary>
        /// <param name="path">Configuration path to check.</param>
        /// <returns>True if the path exists, false otherwise.</returns>
        public static bool Exists(string path)
        {
            lock (_lock)
            {
                var section = _config.GetSection(path);
                return section.Exists();
            }
        }

        /// <summary>
        /// Gets a URL by name from the Urls section.
        /// </summary>
        /// <param name="name">URL name as defined in config.</param>
        /// <returns>The URL as Uri.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when URL name is not found.</exception>
        public static Uri GetUrl(string name)
        {
            lock (_lock)
            {
                var urlString = _config.GetValue<string>($"Urls:{name}");
                if (string.IsNullOrEmpty(urlString))
                {
                    var availableUrls = GetSectionAsDictionary("Urls");
                    throw new KeyNotFoundException(
                        $"URL with name '{name}' not found in configuration. " +
                        $"Available URLs: {string.Join(", ", availableUrls.Keys)}");
                }
                return new Uri(urlString);
            }
        }

        /// <summary>
        /// Tries to get a URL by name from the Urls section.
        /// </summary>
        /// <param name="name">URL name as defined in config.</param>
        /// <param name="url">The URL if found.</param>
        /// <returns>True if URL was found, false otherwise.</returns>
        public static bool TryGetUrl(string name, [NotNullWhen(true)] out Uri? url)
        {
            lock (_lock)
            {
                url = null;
                var urlString = _config.GetValue<string>($"Urls:{name}");
                if (!string.IsNullOrEmpty(urlString))
                {
                    url = new Uri(urlString);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Navigates the browser to a URL by name.
        /// </summary>
        /// <param name="urlName">URL name as defined in config.</param>
        public static void NavigateTo(string urlName)
        {
            var url = GetUrl(urlName);
            TestExecutionContext.BrowserActions.NavigateToUrl(url);
        }

        // Returns the built root rather than assigning the field directly: the static constructor
        // then contains a visible assignment to _config, which is what lets the compiler prove the
        // field is initialized without a null-forgiving operator.
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(BaseConfigFileName, optional: true, reloadOnChange: false);

            if (!string.IsNullOrEmpty(_environment))
            {
                var envConfigFile = $"config.{_environment}.json";
                builder.AddJsonFile(envConfigFile, optional: true, reloadOnChange: false);
            }

            return builder.Build();
        }
    }
}
