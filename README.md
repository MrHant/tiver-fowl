# tiver-fowl
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMrHant%2Ftiver-fowl.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FMrHant%2Ftiver-fowl?ref=badge_shield)


A framework for writing Automated Integration tests (including tests via Selenium).

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
* Add unit-testing framework like MSTest or NUnit and add a BaseClass (refer to ['BaseTestForMsTest.cs.pp'](package/BaseTestForMsTest.cs.pp) or ['BaseTestForNUnit.cs.pp'](package/BaseTestForNUnit.cs.pp))
* Write up some tests and you are ready to go

### Package update

* Please make sure to use Version Control System and commit all your changes before package update
* Package update will recreate all package-specific files - like elements and templates
* Please review changes and run your tests after package update

## Local execution

* To run tests locally - Select "Default.runsettings" as configuration file for Tests
 * It contains settings for output folder
 * As well as copying driver executables


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMrHant%2Ftiver-fowl.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FMrHant%2Ftiver-fowl?ref=badge_large)