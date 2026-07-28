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
| `Resolution` | object | `{ "Width": 1200, "Height": 800 }` |
| `DriverManager` | string | `SeleniumManager` (default), `TiverFowlDrivers`, or `None` |
| `RunningInDocker` | bool | Defaults to `false`; applies container-friendly browser switches |
| `RemoteAddress` | Uri | Set to use a remote WebDriver (Selenium Grid) instead of a local browser |

Bound by `ConfigurationMapper` into `BrowserConfiguration`; consumed by `BrowserFactory.GetBrowser()`.

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
