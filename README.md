# tiver-fowl

A framework for writing Automated Integration tests (including tests via Selenium).

## Installation

* Add [Tiver.Fowl nuget package](https://www.nuget.org/packages/Tiver.Fowl/) to your project with tests, following changes will be done:
 * Project libraries referenced, as well as MSTest and other project dependencies (via NuGet packages)
 * Sample config file ['App.config.tiver.fowl.sample'](package/App.config.tiver.fowl.sample) created
 * ['AssemblyCleanup.cs'](package/AssemblyCleanup.cs.pp) created
 * Driver executables copied to 'lib' folder
 * Report templates copied to 'templates' folder
* Add needed configuration options to 'App.config' (refer to ['App.config.tiver.fowl.sample'](package/App.config.tiver.fowl.sample))
* Write up some tests and you are ready to go

## Local execution

* To run tests locally - Select "Default.runsettings" as configuration file for Tests
 * It contains settings for output folder
 * As well as copying driver executables
