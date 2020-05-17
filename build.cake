#tool nuget:?package=ReportGenerator&version=4.5.8

var target = Argument("target", "Build");
Task("Build")
  .Does(() =>
{
  DotNetCoreBuild("./src/cmd.sln");
});

Task("Tests")
  .IsDependentOn("Build")
  .Does(() =>
{
  DotNetCoreTest("./src/cmd.sln");
});

Task("Coverage")
  .IsDependentOn("Build")
  .Does(() =>
{
  var settings = new DotNetCoreTestSettings{
    NoBuild = true,
    Settings = "./coverlet.runsettings"
  };
  DotNetCoreTest("./src/cmd.sln", settings);
  
  var reportSettings = new ReportGeneratorSettings
  {
    ReportTypes = new [] { ReportGeneratorReportType.Html, ReportGeneratorReportType.TextSummary }
  };
  ReportGenerator("./testresults/*/*.xml", "./coverage", reportSettings);
});

Task("Pack")
  .Does(() =>
{
  DotNetCorePack("./src/cmd/cmd.csproj");
});

RunTarget(target);
