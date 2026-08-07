# Philosophy

Why Tiver.Fowl exists, what it believes, and what it deliberately refuses to do.

## The goal

> Let a .NET developer write a browser test that reads like the manual test case — focuses 
> on actions to be performed, never goes stale, runs in parallel, and produces
> a shareable HTML report, with no configuration beyond a JSON file.

Everything below is downstream of that sentence. Each principle is a constraint accepted in order to
keep the test body free of automation plumbing.

## The six principles

### 1. Elements are descriptions, not handles

An `Element` is a locator plus a human-readable name. It is never an `IWebElement`. Every
interaction re-resolves the locator against the live DOM *inside* the retry loop
([`Element.Process`](../Tiver.Fowl/ViewBase/Element.cs)), which means a stale element reference is
not something the framework recovers from — it is a state the framework cannot enter.

This is the load-bearing decision. Because an element carries no driver state, an element instance is
just data: safe to hold as a `static` field, safe to share between tests, safe to declare wherever it
reads best. That is what makes static `View` classes and `[Parallelizable(ParallelScope.All)]` true
at the same time, which in most page-object frameworks are mutually exclusive.

```csharp
// Declared once, at class level, shared across every test in the run — and still parallel-safe.
private static readonly Button LaptopsMenuItem = new("//a[text()='Laptops']", "Laptops");
```

### 2. Waiting is not the test author's job

Every operation runs inside `Wait.Until` with a configured timeout, polling interval, and ignored
exception types. `Thread.Sleep` and explicit waits are not idioms to be used sparingly here; they are
absent, including from custom elements — `Process()` hands your own code the same retry policy the
built-in behaviors get.
If needed, a test can add its own business-logic wait, like for a long-running process to complete - also using `Wait.Until`. 


A test that fails should point at the application, not at a race the author forgot to guard.

### 3. Capabilities are types

```csharp
public class Button : Element, IClickable { }
```

Typical browser actions are a `Behavior` - like `IClickable`, `ITextInput`, and `ISelectable`. A `Button` can be clicked because it has `IClickable`, not because it inherits from a base class that has a click method.

Behaviors are marker interfaces; the methods live in extension methods under
[`ViewBase/Behaviors/Extensions/`](../Tiver.Fowl/ViewBase/Behaviors/Extensions/). Capability is
composed, not inherited, so adding a behavior to the framework never widens the base class and never
grants it to elements that should not have it.

### 4. One locator language

XPath only. This reads as an arbitrary restriction and is in fact the enabling constraint for
everything above.

XPath is the only selector language available through WebDriver that does all three of the following:

- **composes relatively** — `./div/h5` resolves against a parent element, so a compound element can
  describe its own internals without knowing where it sits on the page;
- **indexes positionally** — `(//div[@class='card'])[2]`, without a separate index-into-a-collection
  API;
- **parameterizes cleanly** — `//button[text()='{0}']` is a `string.Format` template, so a
  parameterized element is a locator with a hole in it rather than a locator built by concatenation.

On other hand keeping locators type consistent allows to - keep the locator resolution code simple and uniform.

### 5. The log is the report

Test steps, element actions, screenshots, and results all flow through a single structured Serilog
pipeline. The HTML report is a projection of that log — `LogFileParser` reads the same `log.txt` the
run already produced, and `HtmlReportGenerator` renders it. There is no reporting API to call, no
second instrumentation path to keep in sync with the first, and nothing extra to add to a test to
make it show up in the report.

### 6. Parallel-safe by construction

Per-test state lives in an ambient `StorageScope` published through an `AsyncLocal` and reached via
`Context` / `TestExecutionContext` — never in statics, and never keyed by thread. 

This allows user to store test-specific, access it from different parts of the test classes (like page objects, elements, and test methods) without worrying about cross-test contamination, even when tests are run in parallel.
Also session-specific context is available.

## Non-goals

Intentional decision to not do these things, so that the framework can stay rather small and focused.

- **Not driver-agnostic.** Currently it supports only Selenium for Browser automation.
- **Not a BDD/Gherkin layer.** No feature files, no step bindings. Readability is pursued through the
  element and step API instead.
- **Not a test runner.** NUnit and MSTest run the tests; Tiver.Fowl attaches to their lifecycle.
- **Not record-and-playback.** At this point - No codegen, no recorder, no selector inference.
- **No CSS selectors.** See principle 4.
- **Not multi-targeted.** Current .NET only (net10.0, C# 14). No `netstandard`, no .NET Framework.

## Where the principles live in the docs

| Principle | Detail |
| --- | --- |
| 1, 3, 4 | [USAGE.md](USAGE.md) — elements, behaviors, locators, page objects |
| 2 | [CONFIGURATION.md](CONFIGURATION.md#waitretry-mechanism) — timeout, polling, ignored exceptions |
| 5 | [REPORTING.md](REPORTING.md) — Serilog pipeline, HTML report, templates |
| 6 | [CLAUDE.md](../CLAUDE.md#context-pattern) — the context pattern and its constraints |
