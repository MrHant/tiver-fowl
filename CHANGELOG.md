# Changelog

## [Unreleased]

### Added
- **HTML Test Report Generation**: Self-contained HTML report generated automatically after test execution
  - Parses existing Serilog JSON log file (`log.txt`) - no additional code instrumentation required
  - Includes test name, result (pass/fail/skip), duration, test steps, element actions
  - Tests organized hierarchically by namespace with collapsible sections
- New `Tiver.Fowl.Core.Reporting` namespace with `LogFileParser`, `HtmlReportGenerator`, and data record classes
- `SessionIdEnricher` for Serilog to enrich all logs with session identifiers
- `TestExecutionContext.SessionId` property to access/set the current test session identifier
- `docs/PHILOSOPHY.md` stating the project's goal, six design principles, explicit non-goals. Summarized at the top of
  both READMEs
- `Directory.Build.props` centralizing `TargetFramework`, `LangVersion`, nullable reference types,
  `TreatWarningsAsErrors`, analyzer settings, and shared package metadata across every project.
  NuGet audit warnings (NU1901-NU1904) stay warnings, so a newly published advisory does not break
  an unrelated build
- `Directory.Packages.props` introducing central package management, so each dependency version is
  declared once for the whole repository
- `.editorconfig` encoding the conventions `AGENTS.md` previously described only in prose, including
  block-scoped namespaces with `using` directives inside the namespace body
- Package now publishes `PackageTags` (`selenium`, `webdriver`, `test-automation`, and others), so it
  is discoverable by search on nuget.org where it previously had none
- Package `<repository>` metadata now includes the repository URL via `PublishRepositoryUrl`; it
  previously recorded only a commit hash
- Packages built on CI now set `ContinuousIntegrationBuild`, so symbol files record normalized source
  paths instead of the build agent's absolute paths, making Source Link stepping work for consumers

### Changed
- **BREAKING**: Nullable reference types are enabled across the repository. The public API is now
  annotated, so consumers compiling with `<Nullable>enable</Nullable>` will see new warnings where
  they treat a nullable result as non-null. Members that became nullable, all of which could already
  return null at runtime: `ActiveConfiguration.Environment`, `ActiveConfiguration.Get<T>`,
  `ApplicationConfiguration.Title`, `BrowserConfiguration.BrowserType`,
  `BrowserConfiguration.RemoteAddress`, `BrowserConfiguration.Resolution`,
  `IBrowserActions.ExecuteScript`, `TestExecutionContext.CurrentTestNameOrNull`,
  `TestResultRecord.ErrorMessage`, `TestResultRecord.StackTrace`,
  `TestResultRecord.ScreenshotBase64`, and `TestResultRecord.CurrentStep`
- **BREAKING**: `IStorage.ReadOrInit<T>` gains a `where T : notnull` constraint. Storage holds values
  as `object` and has no representation for a stored null, so the constraint states a rule the type
  already had. Custom `IStorage` implementations must add it
- **BREAKING**: `IBrowserActions.ExecuteScript` returns `object?`. Custom implementations must match
  the nullability or the compiler reports CS8766
- `IStorage.TryRead<T>` annotates its `out` parameter with `[MaybeNullWhen(false)]` and
  `ActiveConfiguration.TryGetUrl` annotates its `out` parameter with `[NotNullWhen(true)]`, so
  callers no longer need a null check after a `true` result
- `ActiveConfiguration.SetEnvironment` accepts `string?`; passing null or empty clears the
  environment layer, which `BuildConfiguration` already handled
- Fixed: when `BrowserConfiguration.BrowserType` is unset and driver management is
  `TiverFowlDrivers`, the driver downloader received null instead of the browser that would actually
  be launched. It now resolves the default browser first
- **BREAKING**: `Flow.Setup(Type, string, Func<string>)` is now `Flow.Setup(Type, string)`. The test
  key delegate is gone — `Flow.Setup` starts an ambient test scope instead of registering a way to
  compute a key. Custom base classes must drop the third argument. `Flow.Setup` must be called from
  a **synchronous** setup method: it installs the ambient scope, and mutations to the ambient
  execution context do not escape an `async` state machine, so a scope installed in an `async` setup
  would not reach the test body. Both shipped base classes already satisfy this
- **BREAKING**: `Context.SetTestKey(Func<string>)` removed. It has no meaning now that the current
  test is tracked by an ambient scope; `Flow.Setup` starts the scope and `Context.ClearTestContext`
  ends it
- **BREAKING**: `Context.Test` throws `InvalidOperationException` when no test scope is active, where
  it previously created storage on demand under whatever key the delegate returned. Ambient consumers
  that may legitimately run outside a test should use the new `Context.TestOrNull` or
  `TestExecutionContext.CurrentTestNameOrNull`
- **BREAKING**: `IStorage` gains `bool TryRead<T>(string key, out T value)`, a non-throwing
  counterpart to `Read<T>`. Custom `IStorage` implementations must add it
- **BREAKING**: `IStorage.ReadOrAdd` renamed to `ReadOrInit`
- `Storage` now uses `ConcurrentDictionary` for thread safety in parallel tests
- Added generic `Read<T>()` and `ReadOrInit<T>()` methods to `IStorage`
- **BREAKING**: New generic configuration system via `ActiveConfiguration` class (now in `Tiver.Fowl.Core.Configuration` namespace)
  - Supports custom keys with arbitrary nesting using path syntax (e.g., `Get<int>("Database:Timeout")`)
  - Environment-based layering: `config.json` → `config.{env}.json`
  - Environment set via code, `TIVER_ENVIRONMENT` env var, or `Tiver_config.json`
- **BREAKING**: Tests control their own navigation via `ActiveConfiguration.NavigateTo("urlName")`
- `BaseTestForNUnit` now uses `TestContext.CurrentContext.Test.FullName` to include namespace in test identification
  - `BaseTestForMSTest` now uses fully qualified test names (with namespace) for proper report hierarchy
- Reorganized documentation into topic-specific files under `docs/`
- Package `<Description>` and both READMEs now read "The smallest opinionated Selenium layer for .NET
  — no waits, no stale elements, a report out of the box." The previous tagline ("a framework for
  writing Automated Integration tests (including tests via Selenium)") implied driver portability the
  framework does not provide; Selenium is a requirement, not an option
- Updated `Selenium.WebDriver` to 4.46.0
  - **BREAKING**: Selenium.WebDriver 4.44.0 renamed its assembly from `WebDriver` to
    `Selenium.WebDriver`. Any `IgnoredExceptionsTypeNames` entry in the `Tiver.Fowl.Waiting` section
    of `Tiver_config.json` must be updated accordingly — for example
    `"OpenQA.Selenium.NoSuchElementException, WebDriver"` becomes
    `"OpenQA.Selenium.NoSuchElementException, Selenium.WebDriver"`. Unresolvable names are skipped
    silently, so a stale entry stops the exception from being swallowed and element lookups fail on
    the first attempt instead of retrying until `Timeout`

### Fixed
- **Behavior change**: a half-specified `BrowserConfiguration.Resolution` is now a configuration
  error instead of a zero-size window. Setting only `Width` or only `Height` converted the missing
  dimension to `0` and launched a browser with no usable viewport, which then failed every
  interaction with errors pointing nowhere near the config file. Both dimensions are now required
  together — omit `Resolution` entirely to keep the browser's default size — and non-positive
  dimensions such as `"Width": 0` are rejected for the same reason. The check runs before the driver
  is created, so a bad resolution no longer leaves an orphaned browser process behind. Configurations
  that relied on the old zero-filling behavior will now throw
  `IncorrectBrowserConfigurationException` naming the missing dimension
- Browser configuration is no longer discarded when running against a Selenium Grid. Both factories
  built their options object *inside* the local branch and handed `RemoteWebDriver` a bare one, so
  setting `RemoteAddress` silently dropped `Headless` and the container switches — meaning
  headless-on-grid, the most common grid setup there is, launched a headed browser and no
  configuration error was reported. Options are now built once and applied to local and remote
  sessions alike. `RunningInDocker` is honoured on both paths; the `/.dockerenv` auto-detection is
  not, since it describes the machine running the tests rather than the node running the browser, so
  a grid node needing `--no-sandbox` must be told through `RunningInDocker`
- **Behavior change**: `ActiveConfiguration.Get<T>` no longer discards a configured value that
  happens to equal `default(T)`. It compared the value it read against `default(T)` and substituted
  the caller's default when they matched, so "absent" and "present but falsy" were indistinguishable:
  `Get<bool>("Flags:Enabled", true)` returned `true` even where the config file said `false`, and
  `Get<int>("Retries:Max", 3)` returned `3` where the file said `0` — the file was silently
  overruled for exactly the values most often used to switch something off. The default now applies
  when the path carries no value: absent, explicitly `null`, or naming a parent section rather than
  a leaf. Callers relying on the old behavior to treat a configured `false`/`0` as "unset" must now
  remove the key instead. `Get<string>` is unaffected, since `default(string)` is `null` and never
  equalled a configured `""`
- Test context is now genuinely isolated under MSTest parallel execution. The running test was
  identified through a single process-global `Func<string>` that every test's setup overwrote. Under
  NUnit the delegate resolved ambiently through `TestContext.CurrentContext`, so it was correct;
  under MSTest it captured a per-instance value, so as soon as a second test ran setup, *every*
  thread resolved to the last test to start. Concurrent MSTest tests shared a single storage bucket:
  they read each other's `TestName` and `TestStep`, and `Flow.Teardown` fetched and quit another
  test's browser while that test was still using it. `Context` now publishes a per-test `TestScope`
  through an `AsyncLocal`, which follows its own test across thread hops, `await`s and `Task.Run`,
  and cannot bleed into a sibling test reusing the same worker thread. `Tests.MSTest` now runs with
  method-level parallelism and covers this
- Per-test storage is no longer retained after a test that fails before teardown. Storage lived in a
  process-global dictionary keyed by test name, and a test that never reached `Flow.Teardown` left
  its entry — including its `IBrowser` reference — in that dictionary for the rest of the run. The
  scope is now reachable only from the ambient execution context and is collected with the test.
  This also removes a collision between distinct tests that resolve to the same name, such as an
  NUnit test re-run by `[Retry]`
- Serilog events emitted outside a test scope keep their other enriched properties. `TestNameEnricher`
  read `TestExecutionContext.TestName`, which throws when no test name has been written — during
  session setup, report generation and teardown. Serilog swallows enricher exceptions into `SelfLog`,
  so the failure was silent. The enricher now reads through a non-throwing accessor and simply omits
  `TestName` when there is no active test
- HTML report generation now works for projects consuming Tiver.Fowl as a NuGet package. The report
  template was packed into `lib/net10.0/`, from which NuGet only flows assemblies into a consumer's
  output directory, so `HtmlReportGenerator` threw `FileNotFoundException` on every run and
  `Flow.SessionTeardown()` swallowed it into a log entry. `report-template.html` is now embedded in
  `Tiver.Fowl.dll` and used whenever no template file is found on disk. A `report-template.html` in
  the output directory still takes precedence, so template customization is unaffected
- **BREAKING**: NUnit session setup no longer runs only for tests under the `Tiver.Fowl.TestingBase`
  namespace. `SetupFixtureForNUnit` was declared inside that namespace, and NUnit scopes a
  `[SetUpFixture]` to its own namespace and that namespace's children — so for package consumers
  neither `Logger.Configure()` nor `Flow.SessionTeardown()` ever ran, leaving them with no `log.txt`
  and no HTML report. It is replaced by `TiverFowlSessionFixture`, declared outside any namespace so
  that it applies to the whole test assembly. Consumers who referenced `SetupFixtureForNUnit` by
  name must update; no change is needed to use it. MSTest was unaffected, as
  `[AssemblyInitialize]`/`[AssemblyCleanup]` are assembly-wide
- Package validation (`Tests.NUnit`, `Tests.MSTest`) no longer restores a stale, previously
  extracted `Tiver.Fowl` package. `Directory.Build.targets` cleared the extracted package using
  `$(NuGetPackageRoot)`, which restore leaves empty because it does not import `nuget.g.props`
  (`ExcludeRestorePackageImports`), making the `RemoveDir` a no-op against a relative path


## [0.2.0-beta]

### Added
- Auto-detection of test framework based on package references
- `BaseTestForNUnit` and `BaseTestForMSTest` base classes shipped as source files
- MSBuild props/targets files for automatic `TIVER_NUNIT`/`TIVER_MSTEST` constant definition
- `Logger` class shipped as source file for easier customization of Serilog configuration

## [0.2.0-alpha]

### Added
- Support for selenium-manager as the default driver management solution.
- New `DriverManager` configuration property in `BrowserConfiguration` with three explicit options:
  - `SeleniumManager` - Uses Selenium's built-in driver management (default, recommended)
  - `TiverFowlDrivers` - Uses Tiver.Fowl.Drivers package for advanced driver control
  - `None` - Manual driver management (assumes drivers in PATH)
- MinVer for automatic semantic versioning from Git tags
- Package README with quick start guide and examples

### Changed
- **BREAKING**: Removed obsolete `DownloadBinary` property from `BrowserConfiguration`. Refer to `DriverManager` instead.
- **BREAKING**: Migrated to .NET 10.0 (dropped .NET 9.0 and .NET Standard 2.0 support)
- **BREAKING**: Updated to C# 14 language version
- SeleniumManager is now the default driver management approach (no configuration required)
- Migrated NuGet package metadata from Package.nuspec into Tiver.Fowl.csproj for simplified packaging


## [0.1.7]

### Changed
- Replaced LibLog with Serilog for logging throughout the framework
- Updated to latest Serilog packages

### Added
- Remote WebDriver support via `remoteAddress` parameter
- TestNameEnricher moved from package to core library


## [0.1.6.1]

### Fixed
- Various NuGet package metadata and file path corrections


## [0.1.6]
### Added
- Automatic driver download enabled by default
- Tiver.Fowl.Drivers package integration (replaced static ChromeDriver binary)
- Tiver.Fowl.Waiting package (extracted Wait functionality to separate package)
- CI/CD pipeline setup with Cake, AppVeyor, and GitVersion
- TestNameEnricher and Logger included in NuGet package

### Changed
- Simplified browser and application configuration structure
- Updated Selenium WebDriver and other dependencies


## [v0.1.5.5]

### Added
- Frame switching support for handling iframes
- NUnit support with BaseTest template
- Test framework agnostic design using TestResult enum

### Changed
- Improved element action logging (skip logging for unnamed elements)
- Refactored TestExecutionContext and Storage


## [v0.1.5] 

### Added
- Element behavior interfaces (IClickable, IVisible, ITypeable, IHasAttributes)
- Interface extension methods for element behaviors (replacing inheritance model)
- ITypeable.Enabled method for checking input field state

### Changed
- Refactored element architecture using interface extensions instead of inheritance
- Moved concrete element implementations to NuGet package
- Updated ChromeDriver to v2.25


## Earlier Releases

Initial framework development including:
- Core Selenium WebDriver abstraction (IBrowser, Chrome/Firefox support)
- Element abstraction layer with XPath locators
- Context management for thread-safe test execution
- Configuration system (app.config based)
- Logging with Serilog
- Screenshot capture on test failure
- Wait/retry mechanism for element interactions
- View/Page Object pattern support
