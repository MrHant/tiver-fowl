# Usage Guide

Practical patterns for writing tests with Tiver.Fowl. For configuration see
[CONFIGURATION.md](CONFIGURATION.md); for logs and reports see [REPORTING.md](REPORTING.md).

## Writing a test

```csharp
[WebDriverTest]  // triggers browser creation during Flow.Setup
public class MyTests : BaseTestForNUnit
{
    [Test]
    public void MyTest()
    {
        ActiveConfiguration.NavigateTo("home");  // tests control their own navigation

        this.LogStep("Navigate to section");
        CatalogView.MonitorsMenuItem.Click();

        Assert.That(CatalogView.MonitorsHeader.Displayed(), Is.True);
    }
}
```

Without `[WebDriverTest]` no browser is created, which is what you want for pure unit-style tests of
framework behavior.

## Element abstraction

Elements are objects wrapping an XPath locator and a human-readable name. `Element` already
implements `IVisible`, `IClickable` and `IHasAttributes`; declare additional behaviors on subclasses
as needed.

```csharp
public class Button : Element, IClickable
{
    public Button(string locator, string name = "unnamed") : base(locator, name) { }
}

var button = new Button("//button[@id='submit']", "Submit Button");
button.Click();  // logs the action, waits, finds the element, clicks
```

An `Element` holds only its locator and name — never a Selenium `IWebElement`. Every interaction
re-resolves the locator against the live DOM inside the retry loop, so stale-element concerns do not
apply and element instances are safe to hold, reuse, or declare however you prefer.

### Behaviors

Behaviors are marker interfaces; the methods come from extension methods in
`ViewBase/Behaviors/Extensions/`.

| Interface | Method |
| --------- | ------ |
| `IClickable` | `Click()` |
| `IVisible` | `Displayed()` |
| `ITypeable` | `Type(text)`, `Enabled()` |
| `IHasAttributes` | `GetAttribute(name)` |

### Locator formatting

Locators support `string.Format` placeholders, supplied either at construction or at call time.

```csharp
// Argument supplied per call
new Element("//button[text()='{0}']", "Formatted Button").Click("Submit");

// Argument baked in at construction
new Element("//button[text()='{0}']", "Submit Button", "Submit").Click();
```

A placeholder with no matching argument throws `LocatorFormattingException`.

### XPath only

All element location uses XPath — CSS selectors are not supported.

```csharp
new Element("//div[@class='item']", "Item Element")
```

## Page Object pattern

Group elements into static View classes.

```csharp
public static class LoginView
{
    public static Textbox UsernameField = new Textbox("//input[@name='username']", "Username");
    public static Textbox PasswordField = new Textbox("//input[@name='password']", "Password");
    public static Button LoginButton = new Button("//button[@type='submit']", "Login");
}
```

## Creating a custom element

1. Inherit from `Element`
2. Implement the behavior interfaces you need
3. Add custom methods, using `Process()` to get retry-wrapped access to the underlying `IWebElement`

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

`Process()` has two overloads — `Process<TResult>(Func<IWebElement, TResult>)` for reads and
`Process(Action<IWebElement>)` for actions. Both apply the configured wait/retry policy, so custom
methods get the same resilience as built-in behaviors for free.

## Browser abstraction

Selenium WebDriver sits behind `IBrowser`, obtained through a factory:

```csharp
IBrowser browser = BrowserFactory.GetBrowser();  // uses BrowserConfiguration
```

Implemented by `ChromeBrowser` and `FirefoxBrowser`. Two action surfaces are exposed:

- **`IBrowserActions`** — `NavigateToUrl`, `Refresh`, `Back`, `Forward`, `SwitchToFrame`,
  `SwitchToMainFrame`, `ExecuteScript`, `TakeScreenshot`, `CloseWindow`, `Quit`
- **`IWebElementActions`** — `Find(locator)`, `FindSeveral(locator)`

```csharp
// Named URL from config.json (preferred)
ActiveConfiguration.NavigateTo("dashboard");

// Direct navigation
TestExecutionContext.BrowserActions.NavigateToUrl(new Uri("https://example.com"));

var title = TestExecutionContext.BrowserActions.ExecuteScript("return document.title;");

// Returns void — the screenshot is written straight to the Serilog log as base64,
// where the HTML report picks it up
TestExecutionContext.BrowserActions.TakeScreenshot();
```

## Context and storage

`Context` is where a test keeps state it needs to carry between steps — a created record's ID, a
generated username. Use it instead of static fields: statics are shared by every test running in
parallel, `Context.TestStorage` is not.

```csharp
// Test-level: isolated per test, discarded when the test ends
Context.TestStorage.Write("userId", createdId);
var userId = Context.TestStorage.Read<string>("userId");
var orDefault = Context.TestStorage.ReadOrInit("retries", 0);
if (Context.TestStorage.TryRead<string>("userId", out var id)) { }

// Session-level: shared across all tests in the run, and readable outside a test
Context.SessionStorage.Write("apiToken", token);
```

The storage follows its own test across `await`s and `Task.Run`, so it stays correct in an async
test. It is available from the moment setup runs; reading it outside a test throws
`InvalidOperationException`. `SessionStorage` is shared mutable state across parallel tests — treat
it as read-mostly and keep anything a single test owns in `TestStorage`.

`TestExecutionContext` exposes the ambient state for the current test:

| Property | Description |
| -------- | ----------- |
| `Browser` | Current browser instance |
| `BrowserActions` | Navigate, Refresh, Screenshot, ExecuteScript, … |
| `WebElementActions` | Find elements |
| `TestName`, `TestResult`, `TestStep`, `TestType` | Current test identity and progress |
| `TestStartTime` | Used for duration reporting |
| `SessionId` | Identifies the run; see [REPORTING.md](REPORTING.md) |
| `IsWebDriverTest` | Whether the current test carries `[WebDriverTest]` |

## Parallel execution

Supported on both test frameworks:

- **NUnit** — `[Parallelizable(ParallelScope.All)]` on test classes
- **MSTest** — `[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]`
  (`Workers = 0` means one worker per processor)

Each test gets its own browser instance and isolated test storage. Session storage is shared — treat
writes to it as shared mutable state.

The current test is tracked by an ambient `AsyncLocal` scope, so the context follows its own test
across thread hops: `async` test methods, code after an `await`, and work spawned on `Task.Run` all
resolve to the correct test.

One requirement follows from how ambient scopes work: **`Flow.Setup(...)` must be called from a
synchronous setup method.** Mutations to the ambient execution context do not escape an `async`
state machine, so a scope installed inside an `async` setup would not reach the test body. Both
shipped base classes already use synchronous setup, so this only matters if you write your own. It
fails loudly — accessing the context without an active scope throws `InvalidOperationException`
rather than silently returning another test's state. The test body itself may be `async` freely.

## Screenshot on failure

Captured automatically in `Flow.Teardown()` when a test fails, logged with `LogType: "Screenshot"`
and base64-encoded into the JSON log, from which the HTML report embeds it.
