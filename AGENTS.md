# Repository Guidelines

## Project Structure & Module Organization

`Tiver.Fowl/` contains the .NET 10 framework. Core configuration, context, reporting, and exceptions live under `Core/`; Selenium wrappers are in `WebDriverExtended/`; reusable element abstractions are in `ViewBase/`; and lifecycle helpers are in `TestingBase/`. Source files shipped inside the NuGet package are under `Tiver.Fowl/contentFiles/`.

`Tests/` is the main NUnit suite and includes framework tests, sample views/elements, and explicit report demos. `Tests.NUnit/` and `Tests.MSTest/` validate the packed consumer experience. `TestContentFiles/` ensures packaged source files compile. See `TESTING.md` for the three-layer validation design.

## Build, Test, and Development Commands

The repository uses .NET SDK `10.0.100` and optionally [go-task](https://taskfile.dev/):

- `task restore` — restore solution dependencies.
- `task build` — compile `Tiver.Fowl.slnx` and validate content files.
- `task test-main` — run the primary NUnit suite.
- `task tests` — run main, NUnit consumer, and MSTest consumer suites.
- `task pack` — create a Release NuGet package using the version derived by MinVer.
- `task report-demo` — generate an HTML report; one demo failure is intentional.

Equivalent `dotnet` commands are listed in `Taskfile.yml`. Run a focused test with `dotnet test Tests/Tests.csproj --filter "FullyQualifiedName~ContextTests"`.

## Coding Style & Naming Conventions

Use C# 14, four-space indentation, braces on new lines, and file-scoped namespaces where consistent with the file being edited. Use PascalCase for types, methods, and public members; camelCase for locals and parameters; and `_camelCase` for private fields. Keep interfaces prefixed with `I` and exception types suffixed with `Exception`. Match nearby style because no repository-wide formatter configuration is committed.

## Testing Guidelines

Use NUnit for `Tests/` and name tests descriptively, typically `Member_ExpectedBehavior`. Add regression coverage under `Tests/FrameworkTests/`. Changes to packaged templates or `contentFiles` must pass both `task test-nunit` and `task test-mstest`. Browser-dependent tests should remain headless and use the checked-in JSON configuration.

## Changelog

Update `CHANGELOG.md` for any change that affects end users — public API, configuration, packaging, or observable behavior. Add entries under `[Unreleased]`, creating the category heading (`Added`, `Changed`, `Fixed`, `Removed`, `Deprecated`, `Security`) if it is not already present. Prefix breaking changes with `**BREAKING**:` as existing entries do, and name the affected type or member so entries stay searchable. Purely internal refactors, test-only changes, and formatting do not need an entry. Versions are derived from Git tags by MinVer (see `VERSIONING.md`), so leave everything under `[Unreleased]` until a release is tagged — never add a version heading by hand.

## Commit & Pull Request Guidelines

Recent commits use short, imperative, mostly lowercase subjects such as `update dependencies` and `refactor report tests`. Keep each commit focused. Never commit credentials in `config*.json` or generated artifacts from `bin/`, `obj/`, `test-packages/`, or test reports.
