# Changelog

## [Unreleased]

### Changed
- **BREAKING**: New generic configuration system via `ActiveConfiguration` class (now in `Tiver.Fowl.Core.Configuration` namespace)
  - Supports custom keys with arbitrary nesting using path syntax (e.g., `Get<int>("Database:Timeout")`)
  - Environment-based layering: `config.json` → `config.{env}.json`
  - Environment set via code, `TIVER_ENVIRONMENT` env var, or `Tiver_config.json`
- **BREAKING**: Tests control their own navigation via `ActiveConfiguration.NavigateTo("urlName")`

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
