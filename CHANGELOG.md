# Changelog

## [Unreleased]

### Added
- Support for selenium-manager as the default driver management solution.
- New `DriverManager` configuration property in `BrowserConfiguration` with three explicit options:
  - `SeleniumManager` - Uses Selenium's built-in driver management (default, recommended)
  - `TiverFowlDrivers` - Uses Tiver.Fowl.Drivers package for advanced driver control
  - `None` - Manual driver management (assumes drivers in PATH)

### Changed
- **BREAKING**: Removed obsolete `DownloadBinary` property from `BrowserConfiguration`. Refer to `DriverManager` instead.
- SeleniumManager is now the default driver management approach (no configuration required)


## [0.1.x] - Previous Releases

Initial releases with basic functionality. See git history for details.
