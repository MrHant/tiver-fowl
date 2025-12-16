# Testing Guide for Tiver.Fowl

This document describes the testing decisions and validation strategy for the Tiver.Fowl framework.

## Test Projects Overview

| Project | Purpose | Reference Type |
|---------|---------|----------------|
| **Tests** | Main development test suite | ProjectReference to Tiver.Fowl |
| **Tests.NUnit** | Package validation (NUnit integration) | PackageReference to Tiver.Fowl |
| **Tests.MSTest** | Package validation (MSTest integration) | PackageReference to Tiver.Fowl |
| **TestContentFiles** | ContentFiles compilation validation | Direct file links to contentFiles |

## The ContentFiles Challenge

The framework ships source files (`BaseTestForNUnit.cs`, `BaseTestForMSTest.cs`, `Logger.cs`) as **contentFiles** that compile in the consumer's project. These files:
- Are excluded from Tiver.Fowl compilation in Debug/Release builds
- Are packaged as source code in the NuGet package
- Compile in the consumer's project when they install the package

**Risk**: Syntax errors or breaking changes in contentFiles won't be discovered until users install the package and build their projects.

## Three-Layer Validation Strategy

### Layer 1: TestContentFiles - Immediate Feedback
**Decision**: Validate contentFiles on every build during development.

**What's verified**:
- ContentFiles compile successfully
- Both `#if TIVER_NUNIT` and `#if TIVER_MSTEST` code paths are syntactically correct
- Auto-detection via `Tiver.Fowl.targets` works correctly

**When**: Every `dotnet build` (TestContentFiles is part of the solution)

### Layer 2: Development Configuration - Pre-Package Validation
**Decision**: Create a special build configuration that mimics the packaging environment.

**What's verified**:
- ContentFiles compile alongside framework code without conflicts
- Both NUnit and MSTest framework integrations compile together
- No missing references or dependencies

**How it works**:
- `Development` configuration disables default file inclusion
- Explicitly compiles all source files including contentFiles
- References both test frameworks
- Automatically triggered before package validation tests

**Configuration strategy**:
- **Debug/Release**: Normal builds, contentFiles excluded (not compiled)
- **Development**: Special build for validation, contentFiles included (compiled)

### Layer 3: Package Validation - Real User Experience
**Decision**: Test actual packaged NuGet packages, not just ProjectReferences.

**What's verified**:
- NuGet package structure is correct
- ContentFiles are extracted to consumer projects
- Framework auto-detection identifies NUnit vs MSTest correctly
- Tests run successfully using packaged contentFiles

**How it works** (automated via `Directory.Build.targets`):
- Before restore: Build Tiver.Fowl in Development config (validates contentFiles)
- Clear NuGet cache to prevent stale packages
- Pack (Release config) with test-specific versions (`0.0.1-testingnunit`, `0.0.1-testingmstest`)
- Restore package from local `test-packages/` directory
- Build and run tests using the packaged contentFiles

**Why separate packages**: Each test project gets a unique package version to prevent cache conflicts.

## Running Tests

**Development (fast feedback)**:
- `dotnet test Tests/Tests.csproj` - Main test suite
- `dotnet build` - Automatically validates contentFiles via TestContentFiles

**Package validation (pre-release)**:
- `dotnet build Tests.NUnit/Tests.NUnit.csproj` - Validates NUnit package
- `dotnet test Tests.NUnit/Tests.NUnit.csproj` - Runs tests via package
- `dotnet build Tests.MSTest/Tests.MSTest.csproj` - Validates MSTest package
- `dotnet test Tests.MSTest/Tests.MSTest.csproj` - Runs tests via package

## Key Design Decisions

### Automation via Directory.Build.targets
**Decision**: Automate the entire validation pipeline to prevent manual errors.

**What it does**:
- Intercepts restore for Tests.NUnit and Tests.MSTest projects
- Builds Tiver.Fowl in Development configuration (validates contentFiles)
- Clears NuGet cache for test versions
- Packs fresh package with unique version
- Restores the fresh package for testing

**Benefit**: No manual pack/clear/restore steps; contentFiles always validated before package tests.

### Test Framework Auto-Detection
**Decision**: Auto-detect NUnit vs MSTest rather than requiring manual configuration.

**Implementation**: `Tiver.Fowl.targets` inspects PackageReference items and defines `TIVER_NUNIT` or `TIVER_MSTEST` constants before compilation.

**Benefit**: ContentFiles automatically compile the correct code path for the consumer's test framework.

### Separate Package Versions
**Decision**: Use unique package versions for NUnit and MSTest validation (`0.0.1-testingnunit`, `0.0.1-testingmstest`).

**Benefit**: Prevents NuGet cache conflicts when switching between test projects.

## Troubleshooting

**Stale package cache**: Clear with `dotnet nuget locals all --clear` and delete `~/.nuget/packages/tiver.fowl/0.0.1-testing*`

**ContentFiles not compiling**: Verify Development config builds: `dotnet build Tiver.Fowl/Tiver.Fowl.csproj -c Development`

**Package test failures**: Check if contentFiles have errors by building Development config directly.

## Why This Approach?

1. **Fast feedback**: Layer 1 catches errors on every build
2. **Pre-package validation**: Layer 2 validates before creating packages
3. **Real user testing**: Layer 3 tests actual packaged experience
4. **Zero manual steps**: Fully automated via MSBuild targets
5. **Dual framework support**: Both NUnit and MSTest paths validated
6. **No false positives**: Fresh packages on every test run
