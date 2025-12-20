# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Tiver-Fowl** is a .NET framework for writing automated integration tests, with primary focus on Selenium-based web automation. The framework provides element abstraction, configuration management, logging, and test lifecycle management. It targets .NET 10.0.

**Repository**: https://github.com/MrHant/tiver-fowl
**Current SDK**: .NET 10.0.100

## Build and Test Commands

### Build
```bash
# Build entire solution
dotnet build /workspaces/tiver-fowl/Tiver.Fowl.sln

# Build specific projects
dotnet build /workspaces/tiver-fowl/Tiver.Fowl/Tiver.Fowl.csproj
dotnet build /workspaces/tiver-fowl/Tests/Tests.csproj
```

### Run Tests
```bash
# Run all tests
dotnet test /workspaces/tiver-fowl/Tests/Tests.csproj

# Run specific test by name
dotnet test /workspaces/tiver-fowl/Tests/Tests.csproj --filter "FullyQualifiedName~SampleTests"

# Run with detailed logging
dotnet test /workspaces/tiver-fowl/Tests/Tests.csproj --verbosity detailed
```

### Test Framework Support
The framework supports both **NUnit** and **MSTest**. Base classes are shipped as source files that compile in your project. The test framework is **auto-detected** based on your package references.

**For NUnit:**
1. Add NUnit package reference to your test project
2. Inherit tests from `BaseTestForNUnit`

**For MSTest:**
1. Add MSTest.TestFramework package reference to your test project
2. Inherit tests from `BaseTestForMSTest`

The framework automatically defines `TIVER_NUNIT` or `TIVER_MSTEST` based on which package you reference. No manual configuration needed.

The Tests project in this repo uses **NUnit 4.4.0**:
- Supports parallel test execution with `[Parallelizable(ParallelScope.All)]`
- Tests inherit from `BaseTestForNUnit` which handles Setup/Teardown

## Solution Structure

### Main Projects
- **Tiver.Fowl/** - Core framework library (net10.0)
- **Tests/** - Test project with examples and framework tests (net10.0)

### Key Directories in Tiver.Fowl
- **Core/** - Configuration, context management, attributes, exceptions
  - `Attributes/` - `[WebDriverTest]` attribute marks tests requiring browser
  - `Configuration/` - BrowserConfiguration, ApplicationConfiguration, ConfigurationMapper
  - `Context/` - Thread-safe test execution context and storage
- **TestingBase/** - `Flow` class managing test lifecycle (Setup/Teardown)
- **ViewBase/** - Element abstraction and behavior interfaces
  - `Behaviors/` - IClickable, IVisible, ITypeable, IHasAttributes
  - `Behaviors/Extensions/` - Extension methods providing actual behavior implementations
- **WebDriverExtended/** - Selenium abstraction
  - `Browsers/` - IBrowser interface, Browser base class, Chrome/Firefox implementations, BrowserFactory
- **Logging/** - Serilog integration, element logging, test name enricher

## Architecture Overview

### Test Execution Flow

1. **[SetUpFixture] OneTimeSetUp** - Session initialization
   - Calls `Logger.Configure()` to set up Serilog and wait module

2. **[SetUp] Setup** - Before each test
   - Calls `Flow.Setup(testType, testName, testKeyFunc)`
   - If test has `[WebDriverTest]` attribute:
     - Reads `BrowserConfiguration` from `Tiver_config.json`
     - Creates browser via `BrowserFactory.GetBrowser()`
     - Automatically downloads browser driver via Selenium Manager (default) or optionally via Tiver.Fowl.Drivers
   - **Note**: Tests control their own navigation via `ActiveConfiguration.NavigateTo("urlName")`

3. **[Test] Test Method** - Test execution
   - Access current browser via `TestExecutionContext.Browser`
   - Use element abstractions (Button, Textbox, etc.)
   - All element interactions include automatic wait/retry

4. **[TearDown] Teardown** - After each test
   - Calls `Flow.Teardown(testResult)`
   - Takes screenshot on failure
   - Quits browser
   - Clears test context

5. **[SetUpFixture] OneTimeTearDown** - Session cleanup
   - Calls `Flow.SessionTeardown()`

### Context Pattern

Thread-safe context management using `Context` class:
- **Session-level storage** - Shared across all tests
- **Test-level storage** - Isolated per test
- Access via `TestExecutionContext` properties:
  - `TestExecutionContext.Browser` - Current browser instance
  - `TestExecutionContext.BrowserActions` - Navigate, Refresh, Screenshot, etc.
  - `TestExecutionContext.WebElementActions` - Find elements
  - `TestExecutionContext.TestName`, `TestResult`, `TestStep`

### Element Abstraction

Elements are represented as objects with XPath locators:

```csharp
// Element definition
public class Button : Element, IClickable
{
    public Button(string locator, string name = null) : base(locator, name) { }
}

// Usage
var button = new Button("//button[@id='submit']", "Submit Button");
button.Click();  // Logs action, waits, finds element, clicks
```

**Key behaviors** (marker interfaces + extensions):
- `IClickable` → `Click()` method via extension
- `IVisible` → `Displayed()` method
- `ITypeable` → `Type(text)` method
- `IHasAttributes` → `GetAttribute(name)` method

**Locator formatting** - Supports parameterized locators:
```csharp
new Element("//button[text()='{0}']", "Formatted Button").Click("Submit");
```

### Browser Abstraction

Selenium WebDriver wrapped behind `IBrowser` interface:
- `IBrowserActions` - Navigate, Refresh, Back, Forward, SwitchToFrame, TakeScreenshot, ExecuteScript
- `IWebElementActions` - Find(locator), FindSeveral(locator)

Implemented by `ChromeBrowser` and `FirefoxBrowser` via factory pattern:
```csharp
IBrowser browser = BrowserFactory.GetBrowser();  // Uses configuration
```

### Configuration System

Two configuration files required:

**Tiver_config.json** - Framework configuration:
```json
{
  "BrowserConfiguration": {
    "BrowserType": "chrome",  // or "firefox"
    "Headless": true,
    "Resolution": { "Width": 1200, "Height": 800 },
    "DriverManager": "SeleniumManager"  // Optional: "SeleniumManager" (default), "TiverFowlDrivers", or "None"
  },
  "Tiver.Fowl.Drivers": { ... },  // Only needed when DriverManager="TiverFowlDrivers"
  "Tiver.Fowl.Waiting": { ... }    // Timeout, polling, ignored exceptions
}
```

**Driver Management Options:**
- `"SeleniumManager"` (default) - Uses Selenium's built-in driver management. No additional configuration needed. **Recommended**.
- `"TiverFowlDrivers"` - Uses Tiver.Fowl.Drivers package for more control over driver versions. Requires `Tiver.Fowl.Drivers` configuration section.
- `"None"` - No automatic driver management. Assumes drivers are already in PATH.

**config.json** - Test-specific configuration (generic, supports any structure):
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

**Environment-based configuration layering:**
Files are loaded in order (later overrides earlier):
1. `config.json` - base configuration
2. `config.{environment}.json` - environment-specific overrides (optional)

Environment is resolved in priority order:
1. `ActiveConfiguration.SetEnvironment("qa")` - code override (highest priority)
2. `TIVER_ENVIRONMENT` environment variable
3. `"Environment"` key in `Tiver_config.json` (lowest priority)

```json
// Tiver_config.json - set default environment
{
  "Environment": "qa",
  "BrowserConfiguration": { ... }
}
```

```csharp
// Override environment in code (reloads configuration)
ActiveConfiguration.SetEnvironment("prod");

// Check current environment
var env = ActiveConfiguration.Environment;
```

**Accessing configuration values:**
```csharp
// Get typed values using colon-separated paths
var timeout = ActiveConfiguration.Get<int>("Database:Connection:Timeout");
var connStr = ActiveConfiguration.Get<string>("Database:Connection:String");

// Get with default value
var retries = ActiveConfiguration.Get<int>("Retries", defaultValue: 3);

// Bind section to object
var dbConfig = ActiveConfiguration.GetSection<DatabaseConfig>("Database");

// Get section as dictionary
var urls = ActiveConfiguration.GetSectionAsDictionary("Urls");

// Check if key exists
if (ActiveConfiguration.Exists("FeatureFlags:NewUI")) { ... }

// URL convenience methods (backward compatible)
ActiveConfiguration.NavigateTo("home");  // Navigate browser to named URL
var url = ActiveConfiguration.GetUrl("login");  // Get URL as Uri
```

Both files copied to output directory via .csproj configuration.

### Logging System

Uses **Serilog** with structured logging:
- Logs to console and JSON file (`log.txt` in test directory)
- Test name automatically added via `TestNameEnricher`
- Element actions logged via `ILoggableElementExtensions.LogAction()`
- Screenshots logged as base64 strings

Log types:
- "TestResult" - Test pass/fail
- "ElementAction" - Click, Type, etc.
- "Screenshot" - Base64 image data
- "TestStep" - Custom step logging via `this.LogStep("message")`

### HTML Test Reporting

Uses **automatic HTML report generation** from Serilog structured log files:
- Reports generated automatically in `Flow.SessionTeardown()` after all tests complete
- Parses `log.txt` file to extract test execution data (no code instrumentation needed)
- Self-contained single HTML file with embedded CSS/JS
- Each session generates separate report: `test-report-{SessionId}.html`

**Report Contents**:
- Summary statistics (total, passed, failed, skipped, duration)
- Tests grouped by namespace with collapsible sections
- For each test: name, result, duration, steps, element actions
- Screenshots embedded as base64 for failed tests
- Interactive filtering by result (all/passed/failed/skipped)
- Search by test name

**Report Structure**:
```
Test Report
├─ Summary: Total/Passed/Failed/Skipped/Duration
├─ Filters: All | Passed | Failed | Skipped | Search
└─ Namespaces (collapsible)
    └─ Tests (click to expand)
        ├─ Error message & stack trace (if failed)
        ├─ Screenshot (if failed)
        └─ Steps & Element Actions
```

**Session Identification**:
- SessionId format: `yyyyMMdd-HHmmss-fff-RRRR` (date, time, milliseconds, random suffix)
- Set automatically in `Logger.Configure()` via `TestExecutionContext.SessionId`
- Used to separate multiple test runs in accumulated `log.txt` file
- Milliseconds and random suffix prevent collisions in parallel/rapid executions

**Template Customization**:
- Template file: `Tiver.Fowl/Core/Reporting/report-template.html`
- Copied to output directory via `.csproj` configuration
- Uses `{{PLACEHOLDER}}` tokens: `{{TITLE}}`, `{{SUMMARY}}`, `{{TESTS}}`, `{{FOOTER}}`
- **Styling**: Uses Tailwind CSS and DaisyUI loaded via CDN for modern, responsive design
- Customize appearance by editing Tailwind/DaisyUI classes or adding custom CSS in template `<style>` section
- Template loaded from multiple fallback locations (current dir, base dir, assembly locations)

**Related Classes**:
- `LogFileParser` - Parses Serilog JSON log entries with validation and error logging
- `HtmlReportGenerator` - Template-based HTML generation with robust path resolution
- `TestResultRecord`, `TestStepRecord`, `ElementActionRecord` - Data models
- `SessionIdEnricher` - Adds SessionId to all log events

**Troubleshooting**:
- If report not generated, check console for "Failed to generate test report" error
- Template must be in output directory (check `.csproj` has `<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>`)
- If parsing fails, check `log.txt` contains valid JSON (one object per line)
- Malformed JSON entries are logged as warnings and skipped (partial parsing supported)
- If template not found, error message lists all searched locations

### Wait/Retry Mechanism

All element interactions use `Tiver.Fowl.Waiting` package:
- Element `Process()` methods wrap Selenium calls with automatic retry
- Configurable timeout, polling interval, ignored exceptions
- Configured in `Tiver_config.json` under `Tiver.Fowl.Waiting` section

## Development Notes

### Changelog
Always update `CHANGELOG.md` for meaningful changes that affect end-users. Add entries under the `[Unreleased]` section using the appropriate category (Added, Changed, Fixed, etc.).

### Target Frameworks
- **Tiver.Fowl library**: Targets net10.0
- **Tests project**: Targets net10.0
- **Language version**: C# 14

### Key Dependencies
- Selenium.WebDriver 4.38.0 (includes Selenium Manager for automatic driver management)
- Microsoft.Extensions.Configuration 9.0.10
- Serilog 4.3.0
- Tiver.Fowl.Drivers 0.6.0-alpha.4 (optional - for advanced driver management)
- Tiver.Fowl.Waiting 0.5.0-alpha (wait/retry logic)
- NUnit 4.4.0 (test framework)

### XPath Locators
All element location uses XPath (not CSS selectors). Define locators as:
```csharp
new Element("//div[@class='item']", "Item Element")
```

### Page Object Pattern
Organize elements into View classes:
```csharp
public static class CatalogView
{
    public static Button MonitorsMenuItem => new Button("//a[text()='Monitors']", "Monitors Menu");
}
```

### Creating New Elements
1. Inherit from `Element`
2. Implement behavior interfaces (IClickable, IVisible, etc.)
3. Add custom methods if needed
4. Extensions automatically provide Click(), Displayed(), etc. methods

### Browser Driver Management
**Default (Selenium Manager - Recommended):**
- No configuration required - Selenium automatically downloads and manages drivers
- Works out of the box for Chrome and Firefox
- Automatically detects browser versions and downloads compatible drivers

**Optional (Tiver.Fowl.Drivers):**
- Set `DriverManager: "TiverFowlDrivers"` in BrowserConfiguration
- Provides more control over driver versions and platforms
- Requires `Tiver.Fowl.Drivers` configuration section in Tiver_config.json
- Useful for pinning specific driver versions or custom download configurations

**Manual (None):**
- Set `DriverManager: "None"` in BrowserConfiguration
- Assumes drivers are already available in PATH
- Use when managing drivers through external tools or CI/CD pipelines

### Parallel Test Execution
- Framework supports parallel tests via thread-safe context
- Use `[Parallelizable(ParallelScope.All)]` on test classes
- Each test gets isolated Browser instance and context storage

### Screenshot on Failure
- Automatically captured in `Flow.Teardown()` when test fails
- Logged to Serilog with LogType: "Screenshot"
- Base64 encoded for JSON log file

### DevContainer
Project includes DevContainer with:
- .NET 10.0 SDK (primary)
- Node.js 1.6.3
- Pre-configured VS Code extensions

## Common Patterns

### Creating a Test
```csharp
[WebDriverTest]  // Triggers browser creation
public class MyTests : BaseTestForNUnit
{
    [Test]
    public void MyTest()
    {
        ActiveConfiguration.NavigateTo("home");  // Navigate to starting page

        this.LogStep("Navigate to section");
        SomeView.SomeButton.Click();

        Assert.IsTrue(SomeElement.Displayed());
    }
}
```

### Creating a View (Page Object)
```csharp
public static class LoginView
{
    public static Textbox UsernameField => new Textbox("//input[@name='username']", "Username");
    public static Textbox PasswordField => new Textbox("//input[@name='password']", "Password");
    public static Button LoginButton => new Button("//button[@type='submit']", "Login");
}
```

### Custom Element with Additional Methods
```csharp
public class CatalogItem : Element, IVisible
{
    public CatalogItem(string title)
        : base("//div[contains(@class, 'card') and .//a[text()='{0}']]", title)
    {
    }

    public string GetPrice() => Process(e => e.FindElement(By.XPath(".//h5")).Text);
}
```

### Accessing Browser Actions Directly
```csharp
// Navigate using named URLs (recommended)
ActiveConfiguration.NavigateTo("dashboard");

// Or navigate directly
TestExecutionContext.BrowserActions.NavigateToUrl(new Uri("https://example.com"));
TestExecutionContext.BrowserActions.ExecuteScript("return document.title;");
byte[] screenshot = TestExecutionContext.BrowserActions.TakeScreenshot();
```

### Using Context Storage
```csharp
// Store test-level data
Context.Current.TestStorage.Write("key", value);
var data = Context.Current.TestStorage.Read<T>("key");

// Store session-level data
Context.Current.SessionStorage.Write("key", value);
```
