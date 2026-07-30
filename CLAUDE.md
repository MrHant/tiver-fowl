# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**Read [AGENTS.md](AGENTS.md) first** — it holds the shared repository guidelines (module
organization, build/test commands, coding style, testing expectations, commit conventions) that
apply to every agent working here. This file covers only what is specific to working with Claude
Code on this codebase.

## Project Overview

**Tiver-Fowl** is a .NET framework for writing automated integration tests, with primary focus on
Selenium-based web automation. It provides element abstraction, configuration management, logging,
and test lifecycle management.

- **Repository**: https://github.com/MrHant/tiver-fowl
- **Target framework**: net10.0 (library and all test projects), SDK 10.0.100
- **Language version**: C# 14

## Documentation Map

Load the relevant doc when working in that area rather than assuming behavior:

| Doc | Covers |
| --- | ------ |
| [AGENTS.md](AGENTS.md) | Repository guidelines: structure, commands, style, commits |
| [docs/PHILOSOPHY.md](docs/PHILOSOPHY.md) | Why the project exists, the six principles, non-goals |
| [docs/CONFIGURATION.md](docs/CONFIGURATION.md) | `Tiver_config.json`, `config.json`, environment layering, `ActiveConfiguration` API, driver management, wait/retry |
| [docs/USAGE.md](docs/USAGE.md) | Elements, behaviors, locators, page objects, browser actions, context storage |
| [docs/REPORTING.md](docs/REPORTING.md) | Serilog logging, HTML report generation, templates, troubleshooting |
| [TESTING.md](TESTING.md) | Three-layer validation strategy, contentFiles, NUnit/MSTest package validation |
| [VERSIONING.md](VERSIONING.md) | MinVer-based versioning from Git tags |
| [.devcontainer/README.md](.devcontainer/README.md) | Container image, setup scripts |

## Commands

`Taskfile.yml` wraps the common commands — run `task --list` to see them. The essentials:

```bash
task build       # build Tiver.Fowl.slnx (also validates contentFiles)
task test-main   # main NUnit suite
task tests       # main + NUnit consumer + MSTest consumer suites
task report-demo # generate a sample HTML report (one failure is intentional)
```

Focused test run: `dotnet test Tests/Tests.csproj --filter "FullyQualifiedName~ContextTests"`.

Changes to packaged templates or `contentFiles/` must pass both `task test-nunit` and
`task test-mstest` — see [TESTING.md](TESTING.md) for why.

## Framework Layout

Inside `Tiver.Fowl/`:

- **Core/** — configuration, context, attributes, exceptions, reporting
  - `Attributes/` — `[WebDriverTest]` marks tests requiring a browser
  - `Configuration/` — `ActiveConfiguration`, `BrowserConfiguration`, `ConfigurationMapper`
  - `Context/` — thread-safe test execution context and storage
  - `Reporting/` — `LogFileParser`, `HtmlReportGenerator`, records, `report-template.html`
- **TestingBase/** — `Flow` class managing test lifecycle
- **ViewBase/** — element abstraction; `Behaviors/` holds marker interfaces, `Behaviors/Extensions/`
  holds the actual implementations
- **WebDriverExtended/** — Selenium abstraction; `Browsers/` holds `IBrowser`, Chrome/Firefox
  implementations, `BrowserFactory`
- **Logging/** — Serilog integration, element logging, enrichers
- **contentFiles/** — source files shipped in the NuGet package and compiled in the consumer's
  project (`Logger.cs`, `BaseTestForNUnit.cs`, `BaseTestForMSTest.cs`)

## Test Execution Flow

The lifecycle every test moves through, driven by `Flow`:

1. **OneTimeSetUp** — `Logger.Configure()` sets up Serilog (including `SessionId`) and the wait module
2. **Setup** — `Flow.Setup(testType, testName)`. Starts the ambient test scope, so it must be called
   from a *synchronous* setup method. If the test has `[WebDriverTest]`, reads
   `BrowserConfiguration` and creates a browser via `BrowserFactory.GetBrowser()`. No navigation
   happens here — tests navigate themselves via `ActiveConfiguration.NavigateTo("urlName")`
3. **Test method** — element interactions via the abstraction layer, all wait/retry wrapped
4. **Teardown** — `Flow.Teardown(testResult)`: screenshot on failure, quit browser, clear test context
5. **OneTimeTearDown** — `Flow.SessionTeardown()`, which also generates the HTML report

Test framework (NUnit vs MSTest) is auto-detected from package references, which define
`TIVER_NUNIT` or `TIVER_MSTEST`. The `Tests/` project in this repo uses NUnit.

## Context Pattern

Thread-safe state via the `Context` class, which is what makes parallel execution safe:

- **Session storage** — shared across all tests in a run
- **Test storage** — lives in a `TestScope` published through an `AsyncLocal`, so it is isolated per
  test and follows that test across thread hops and `await`s. Started by `Flow.Setup`, ended in
  teardown
- `TestExecutionContext` exposes the ambient current-test state (`Browser`, `BrowserActions`,
  `WebElementActions`, `TestName`, `TestResult`, `TestStep`, `SessionId`)

Anything added to the framework that holds per-test state must go through `Context`, not statics.
Never key test state by thread — MSTest and NUnit both resume `async` tests on arbitrary pool
threads, so a thread-keyed lookup reads another test's state.

The framework owns this scope rather than keying off the test frameworks' own ambient accessors.
Both `TestContext.CurrentContext` (NUnit) and `TestContext.Current` (MSTest) are `AsyncLocal`-backed
and do resolve the running test, but MSTest's is experimental — `MSTESTEXP` is a compile error by
default, and `BaseTestForMSTest.cs` compiles in the *consumer's* project — needs MSTest 4.x, throws
on `TestName`/`FullyQualifiedTestClassName` outside a test, and reports the same `TestName` for every
`[DataRow]` of a method, so a key derived from it collides across concurrent rows. Ambient consumers that can run outside
a test (log enrichers, session setup/teardown) must use `Context.TestOrNull` /
`TestExecutionContext.CurrentTestNameOrNull` rather than `Context.Test`, which throws when no scope
is active.

## Repository Rules

- **Changelog**: always update `CHANGELOG.md` under `[Unreleased]` for user-affecting changes — see
  [AGENTS.md](AGENTS.md#changelog) for categories, breaking-change format, and what does not need an
  entry.
- **XPath only**: all element location uses XPath, never CSS selectors.
- **Parallel-safe**: the NUnit suites run with `[Parallelizable(ParallelScope.All)]` and
  `Tests.MSTest` with `[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]`; keep new state
  per-test.
- **Headless**: browser-dependent tests stay headless and use the checked-in JSON configuration.
- **Never commit**: credentials in `config*.json`, or generated artifacts from `bin/`, `obj/`,
  `test-packages/`, or test reports.
