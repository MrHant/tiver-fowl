using Microsoft.VisualStudio.TestTools.UnitTesting;

// Runs this consumer suite with real method-level parallelism. Workers = 0 means one worker per
// processor. Beyond exercising the suite faster, this is the configuration that proves Tiver.Fowl's
// ambient test context isolates tests under MSTest — see ParallelIsolationTests.
//
// Deliberately not named AssemblyInfo.cs: .gitignore excludes **/AssemblyInfo.cs, so this
// assembly-wide setting would never be committed under that name.
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
