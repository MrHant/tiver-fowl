# tiver-fowl  ![.NET](https://img.shields.io/badge/.NET-6-blue)  [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/MrHant/tiver-fowl/master/LICENSE) [![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FMrHant%2Ftiver-fowl.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FMrHant%2Ftiver-fowl?ref=badge_shield)

A framework for writing Automated Integration tests (including tests via Selenium).

## Branch status

| Branch | Package | CI  |
| ------ | ------- | --- |
| master (stable) | [![NuGet](https://img.shields.io/nuget/v/Tiver.Fowl.svg)](https://www.nuget.org/packages/Tiver.Fowl) | [![Build status](https://ci.appveyor.com/api/projects/status/6rnoaavfeg192ncd/branch/master?svg=true)](https://ci.appveyor.com/project/MrHant/tiver-fowl/branch/master) |
| develop | [![NuGet Pre Release](https://img.shields.io/nuget/vpre/Tiver.Fowl.svg)](https://www.nuget.org/packages/Tiver.Fowl/absoluteLatest) | [![Build status](https://ci.appveyor.com/api/projects/status/6rnoaavfeg192ncd/branch/develop?svg=true)](https://ci.appveyor.com/project/MrHant/tiver-fowl/branch/develop) |


## Installation

### Clean package install

* Add [Tiver.Fowl nuget package](https://www.nuget.org/packages/Tiver.Fowl/) to your project with tests, following changes will be done:
  * Project libraries referenced, as well as other project dependencies (via NuGet packages)
  * Sample config file ['App.config.tiver.fowl.sample'](package/App.config.tiver.fowl.sample) created
  * Sample BaseClasses created - 'BaseTestForMsTest.cs' and 'BaseTestForNUnit.cs'
  * 'Elements' folder and sample element implementations created
  * Driver executables copied to 'lib' folder
  * Report templates copied to 'templates' folder
* Add needed configuration options to 'App.config' (refer to ['App.config.tiver.fowl.sample'](package/App.config.tiver.fowl.sample))
* Add unit-testing framework (preferably NUnit) and add a BaseClass (refer to ['BaseTestForNUnit.cs.pp'](package/BaseTestForNUnit.cs.pp))
* Write up some tests and you are ready to go

### Package update

* Please make sure to use Version Control System and commit all your changes before package update
* Package update will recreate all package-specific files - like elements and templates
* Please review changes and run your tests after package update

### .NET support
* Targeting .NET 6, .NET Standard 2.0
* Tested with .NET 6, .NET Framework 4.8, .NET Framework 4.7.2, .NET Framework 4.6.2