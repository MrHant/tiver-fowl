# Configuration

Tiver.Fowl uses two JSON configuration files, both copied to the output directory via `.csproj`
configuration:

| File | Purpose |
| ---- | ------- |
| `Tiver_config.json` | Framework configuration — browser, driver management, waiting |
| `config.json` | Test/application configuration — any structure you like |

## Tiver_config.json

```json
{
  "Environment": "qa",
  "BrowserConfiguration": {
    "BrowserType": "chrome",
    "Headless": true,
    "Resolution": { "Width": 1200, "Height": 800 },
    "DriverManager": "SeleniumManager"
  },
  "Tiver.Fowl.Drivers": { },
  "Tiver.Fowl.Waiting": { }
}
```

### BrowserConfiguration

| Property | Type | Notes |
| -------- | ---- | ----- |
| `BrowserType` | string | `chrome` or `firefox` |
| `Headless` | bool | Run without a visible browser window |
| `Resolution` | object | `{ "Width": 1200, "Height": 800 }` — both dimensions required, or omit entirely |
| `DriverManager` | string | `SeleniumManager` (default), `TiverFowlDrivers`, or `None` |
| `RunningInDocker` | bool | Defaults to `false`; applies container-friendly browser switches |
| `RemoteAddress` | Uri | Set to use a remote WebDriver (Selenium Grid) instead of a local browser |

Bound by `ConfigurationMapper` into `BrowserConfiguration`; consumed by `BrowserFactory.GetBrowser()`.

### Remote browsers (Selenium Grid)

Set `RemoteAddress` to run the browser on a grid node instead of the local machine. Every other
property still applies — `Headless`, `Resolution` and the rest configure the browser wherever it runs.

```json
"BrowserConfiguration": {
  "BrowserType": "chrome",
  "Headless": true,
  "Resolution": { "Width": 1200, "Height": 800 },
  "RemoteAddress": "http://localhost:4444/"
}
```

Two properties behave differently against a grid:

- **`RunningInDocker` must be set explicitly** when the node runs in a container. Local runs also
  detect a container automatically by probing for `/.dockerenv`, but that file describes the machine
  running the tests, not the one running the browser.
- **`DriverManager` does not affect the session** — the node supplies its own driver. A
  `TiverFowlDrivers` download still runs on the test machine and then goes unused.

To try this locally:

```bash
docker run -d --shm-size=2g -p 4444:4444 selenium/standalone-chrome
```

### Tiver.Fowl.Waiting

Consumed by the `Tiver.Fowl.Waiting` package, which backs every element interaction. Configures
timeout, polling interval and the exception types to swallow while retrying. See
[Wait/Retry](#waitretry-mechanism) below.

## Browser driver management

Set via `BrowserConfiguration.DriverManager`.

**`SeleniumManager` (default, recommended)**
- No configuration required — Selenium downloads and manages drivers itself
- Works out of the box for Chrome and Firefox
- Detects the installed browser version and fetches a compatible driver

**`TiverFowlDrivers`**
- Uses the `Tiver.Fowl.Drivers` package for control over driver versions and platforms
- Requires a `Tiver.Fowl.Drivers` section in `Tiver_config.json`
- Useful for pinning specific driver versions or custom download configuration

**`None`**
- No automatic driver management; drivers are assumed to be on `PATH`
- Use when drivers are managed by external tooling or the CI/CD pipeline

## config.json

Generic — any structure is supported, and values are read by path.

```json
{
  "Urls": {
    "home": "https://example.com",
    "login": "https://example.com/login"
  },
  "Database": {
    "Connection": {
      "String": "Server=localhost;Database=test",
      "Timeout": 30
    }
  },
  "FeatureFlags": {
    "NewUI": true
  }
}
```

## Environment layering

Files are loaded in order, later overriding earlier:

1. `config.json` — base configuration
2. `config.{environment}.json` — environment-specific overrides (optional)

The active environment is resolved in priority order:

1. `ActiveConfiguration.SetEnvironment("qa")` — code override (highest priority)
2. `TIVER_ENVIRONMENT` environment variable
3. `"Environment"` key in `Tiver_config.json` (lowest priority)

```csharp
// Override in code — reloads configuration immediately
ActiveConfiguration.SetEnvironment("prod");

// Inspect the active environment
var env = ActiveConfiguration.Environment;

// Re-read configuration files without changing environment
ActiveConfiguration.Reload();
```

## Reading configuration values

`ActiveConfiguration` (namespace `Tiver.Fowl.Core.Configuration`) uses colon-separated paths.

```csharp
// Typed values by path
var timeout = ActiveConfiguration.Get<int>("Database:Connection:Timeout");
var connStr = ActiveConfiguration.Get<string>("Database:Connection:String");

// With a default when the key is absent
var retries = ActiveConfiguration.Get<int>("Retries", defaultValue: 3);

// Bind a section to an object
var dbConfig = ActiveConfiguration.GetSection<DatabaseConfig>("Database");

// Section as a flat dictionary
var urls = ActiveConfiguration.GetSectionAsDictionary("Urls");

// Existence check
if (ActiveConfiguration.Exists("FeatureFlags:NewUI")) { }
```

`Get<T>` applies `defaultValue` only when the path carries no value — it is absent, explicitly
`null`, or names a parent section rather than a leaf. A configured value is always returned as
configured, including `false`, `0` and `""`, so `Get<bool>("FeatureFlags:NewUI", true)` returns
`false` when the file says `false`. To express "unset" for a flag, omit the key rather than setting
it to a falsy value.

### URL helpers

```csharp
ActiveConfiguration.NavigateTo("home");           // navigate the current browser to a named URL
var url = ActiveConfiguration.GetUrl("login");    // resolve a named URL, throws if missing

if (ActiveConfiguration.TryGetUrl("staging", out var staging))
{
    // non-throwing variant
}
```

Named URLs are read from the `Urls` section of `config.json`. Tests control their own navigation —
the framework does not navigate anywhere on setup.

## Wait/retry mechanism

Every element interaction goes through the `Tiver.Fowl.Waiting` package:

- `Element.Process()` wraps the underlying Selenium call with automatic retry
- Timeout, polling interval and ignored exception types are configurable
- Configured in `Tiver_config.json` under the `Tiver.Fowl.Waiting` section

```json
"Tiver.Fowl.Waiting": {
  "Timeout": 10000,
  "PollingInterval": 250,
  "IgnoredExceptionsTypeNames": [
    "OpenQA.Selenium.NoSuchElementException, Selenium.WebDriver",
    "OpenQA.Selenium.StaleElementReferenceException, Selenium.WebDriver"
  ]
}
```

`IgnoredExceptionsTypeNames` entries are **assembly-qualified type names** resolved at runtime. A
name that fails to resolve is skipped silently, so the exception is no longer swallowed and the
first failed attempt aborts the retry loop instead of polling until `Timeout`. Watch for element
lookups that fail almost instantly — that is the signature of a stale entry here.

Selenium.WebDriver 4.44.0 renamed its assembly from `WebDriver` to `Selenium.WebDriver`. Configs
written against earlier versions must use the new assembly name, as shown above.
