# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Tiver-Fowl** is a .NET framework for writing automated integration tests, with primary focus on Selenium-based web automation. The framework provides element abstraction, configuration management, logging, and test lifecycle management. It targets .NET 9.0 and .NET Standard 2.0.

**Repository**: https://github.com/MrHant/tiver-fowl
**Current SDK**: .NET 9.0.306

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

### Test Framework
- Uses **NUnit 4.4.0** (not MSTest or xUnit)
- Supports parallel test execution with `[Parallelizable(ParallelScope.All)]`
- Tests inherit from `BaseTestForNUnit` which handles Setup/Teardown

## Solution Structure

### Main Projects
- **Tiver.Fowl/** - Core framework library (multi-targeted: net9.0 + netstandard2.0)
- **Tests/** - Test project with examples and framework tests (net9.0 only)

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
     - Optionally downloads browser binary via Tiver.Fowl.Drivers
     - Navigates to StartUrl from `config.json`

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
    "DownloadBinary": true,    // Auto-download driver
    "Headless": true,
    "Resolution": { "Width": 1200, "Height": 800 }
  },
  "Tiver.Fowl.Drivers": { ... },
  "Tiver.Fowl.Waiting": { ... }  // Timeout, polling, ignored exceptions
}
```

**config.json** - Test-specific configuration:
```json
{
  "StartUrl": "https://example.com"
}
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

### Wait/Retry Mechanism

All element interactions use `Tiver.Fowl.Waiting` package:
- Element `Process()` methods wrap Selenium calls with automatic retry
- Configurable timeout, polling interval, ignored exceptions
- Configured in `Tiver_config.json` under `Tiver.Fowl.Waiting` section

## Development Notes

### Target Frameworks
- **Tiver.Fowl library**: Multi-targeted to net9.0 and netstandard2.0
- **Tests project**: Targets net9.0 only
- **Language version**: C# 13

### Key Dependencies
- Selenium.WebDriver 4.38.0
- Microsoft.Extensions.Configuration 9.0.10
- Serilog 4.3.0
- Tiver.Fowl.Drivers 0.6.0-alpha.4 (browser binary management)
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

### Browser Binary Management
- Set `DownloadBinary: true` in BrowserConfiguration for auto-download
- Tiver.Fowl.Drivers handles platform-specific driver downloads
- Supported: Chrome, Firefox

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
- .NET 10.0, 9.0, and 6.0 SDKs
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
TestExecutionContext.BrowserActions.NavigateToUrl("https://example.com");
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
