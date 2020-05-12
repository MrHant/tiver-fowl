const string project = "Tiver.Fowl";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var projects = Argument(
    "projects",
    "Tiver/Fowl/Core;"
    + "Tiver/Fowl/Logging;"
    + "Tiver/Fowl/TestingBase;"
    + "Tiver/Fowl/ViewBase;"
    + "Tiver/Fowl/WebDriverExtended;"
    + "Tiver/Fowl/WebDriverExtended.Contracts");

var projectDirectories = projects.Split(';');

GitVersion versionInfo;
string version;

Setup(_ =>
{
    Information("");
    Information(@"  _______ _                      ______            _   ");
    Information(@" |__   __(_)                    |  ____|          | |  ");
    Information(@"    | |   ___   _____ _ __      | |__ _____      _| |  ");
    Information(@"    | |  | \ \ / / _ \ '__|     |  __/ _ \ \ /\ / / |  ");
    Information(@"    | |  | |\ V /  __/ |     _  | | | (_) \ V  V /| |  ");
    Information(@"    |_|  |_| \_/ \___|_|    (_) |_|  \___/ \_/\_/ |_|  ");
    Information(@"                                                       ");
    Information("");
});

Teardown(_ =>
{
    Information("Finished running tasks.");
});

Task("RestoreNuGetPackages")
    .Does(() =>
{
    Information("Restoring nuget packages");
    DotNetCoreRestore();
});

Task("Clean")
    .IsDependentOn("RestoreNuGetPackages")
    .Does(() =>
{
    Information("Cleaning project directories");
    CleanDirectories("./artifacts");
    foreach (var dir in projectDirectories) {
        CleanDirectories("./" + dir + "/bin");
        CleanDirectories("./" + dir + "/obj");
    }
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() =>
{
    Information("Building with configuration {0}", configuration);
    var settings = new DotNetCoreBuildSettings
     {
         Framework = "netcoreapp3.1",
         Configuration = configuration,
         OutputDirectory = "./artifacts/"
     };

     foreach (var dir in projectDirectories) {
       DotNetCoreBuild("./"+dir, settings);
     }
});

Task("Version")
    .Does(() =>
{
    GitVersion(new GitVersionSettings{
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.BuildServer,
    });

    versionInfo = GitVersion(new GitVersionSettings{
        OutputType = GitVersionOutput.Json,
    });
    version = versionInfo.LegacySemVerPadded;
});

Task("CreateNuGetPackage")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Packing version {0}", version);
    var nuGetPackSettings = new NuGetPackSettings {
        Version = version,
        OutputDirectory = "./package"
    };

    NuGetPack("./package/Package.nuspec", nuGetPackSettings);
});

Task("PushNuGetPackage")
    .IsDependentOn("CreateNuGetPackage")
    .Does(() =>
{
    var package = "./package/" + project + "."  + version +".nupkg";

    NuGetPush(package, new NuGetPushSettings {
        Source = "https://nuget.org/",
        ApiKey = Environment.GetEnvironmentVariable("NuGet_API_KEY")
    });
});

Task("Default")
    .IsDependentOn("PushNuGetPackage");

RunTarget(target);
