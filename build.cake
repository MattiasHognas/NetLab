var target = Argument("Target", "RunAll");
var configuration =
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") is not null ? EnvironmentVariable("Configuration") :
    "Release";
var artefactsDirectory = Directory("./Artefacts");
var tag =
    HasArgument("Tag") ? Argument<string>("Tag") :
    EnvironmentVariable("Tag") is not null ? EnvironmentVariable("Tag") :
    null;
var platform =
    HasArgument("Platform") ? Argument<string>("Platform") :
    EnvironmentVariable("Platform") is not null ? EnvironmentVariable("Platform") :
    "linux/amd64,linux/arm64";
var push =
    HasArgument("Push") ? Argument<bool>("Push") :
    EnvironmentVariable("Push") is not null ? bool.Parse(EnvironmentVariable("Push")) :
    false;

Task("Clean")
    .Description("Cleans the artefacts, bin and obj directories.")
    .Does(() =>
    {
        CleanDirectory(artefactsDirectory);
        DeleteDirectories(GetDirectories("./src/**/bin"), new DeleteDirectorySettings() { Force = true, Recursive = true });
        DeleteDirectories(GetDirectories("./src/**/obj"), new DeleteDirectorySettings() { Force = true, Recursive = true });
    });

Task("Restore")
    .Description("Restores NuGet packages.")
    .IsDependentOn("Clean")
    .DoesForEach(GetFiles("./src/**/*.sln"), solution =>
    {
        DotNetCoreRestore(solution.GetDirectory().ToString());
    });

Task("Build")
    .Description("Builds the solution.")
    .IsDependentOn("Restore")
    .DoesForEach(GetFiles("./src/**/*.sln"), solution =>
    {

        DotNetCoreBuild(
            solution.GetDirectory().ToString(),
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                NoRestore = true,
            });
    });

Task("Test")
    .Description("Runs unit tests and outputs test results to the artefacts directory.")
    .DoesForEach(GetFiles("./src/services/**/Test/**/*.sln"), project =>
    {
        DotNetCoreTest(
            project.ToString(),
            new DotNetCoreTestSettings()
            {
                Blame = true,
                Collectors = new string[] { "XPlat Code Coverage" },
                Configuration = configuration,
                Loggers = new string[]
                {
                    $"trx;LogFileName={project.GetFilenameWithoutExtension()}.trx",
                    $"html;LogFileName={project.GetFilenameWithoutExtension()}.html",
                },
                NoBuild = true,
                NoRestore = true,
                ResultsDirectory = artefactsDirectory,
            });
    });

Task("Publish")
    .Description("Publishes the solution.")
    .DoesForEach(GetFiles("./src/services/**/Service/**/*.sln"), project =>
    {
        DotNetCorePublish(
            project.ToString(),
            new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                NoBuild = true,
                NoRestore = true,
                OutputDirectory = artefactsDirectory + Directory("Publish"),
            });
    });

Task("Run")
    .Description("Runs all apps and services.")
    .Does(() =>
    {
        DotNetCoreRunInParallel(GetFiles("./src/services/**/Service/**/*.csproj"), "Release");
    });

public void DotNetCoreRunInParallel(
    Cake.Core.IO.FilePathCollection projects,
    string configuration = "Release",
    int maxDegreeOfParallelism = -1,
    System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
{
    var actions = new List<Action>();
    foreach (var project in projects)
    {
        actions.Add(() =>
            DotNetCoreRun(
                project.GetDirectory().ToString(),
                $"-c {configuration}"
            )
        );
    }

    var options = new ParallelOptions
    {
        MaxDegreeOfParallelism = maxDegreeOfParallelism,
        CancellationToken = cancellationToken
    };

    Parallel.Invoke(options, actions.ToArray());
}

Task("DockerBuild")
    .Description("Builds a Docker image.")
    .DoesForEach(GetFiles("./src/services/**/Dockerfile"), dockerfile =>
    {
        tag = tag ?? dockerfile.GetDirectory().GetDirectoryName().ToLower();
        var version = GetVersion();
        var gitCommitSha = GetGitCommitSha();

        // Docker buildx allows you to build Docker images for multiple platforms (including x64, x86 and ARM64) and
        // push them at the same time. To enable buildx, you may need to enable experimental support with these commands:
        // docker buildx create --name builder --driver docker-container --use
        // docker buildx inspect --bootstrap
        // To stop using buildx remove the buildx parameter and the --platform, --progress switches.
        // See https://github.com/docker/buildx
        // StartProcess(
        //     "docker",
        //     new ProcessArgumentBuilder()
        //         .Append("buildx")
        //         .Append("build")
        //         .AppendSwitchQuoted("--platform", platform)
        //         .AppendSwitchQuoted("--progress", BuildSystem.IsLocalBuild ? "auto" : "plain")
        //         .Append($"--push={push}")
        //         .AppendSwitchQuoted("--tag", $"{tag}:{version}")
        //         .AppendSwitchQuoted("--build-arg", $"Configuration={configuration}")
        //         .AppendSwitchQuoted("--label", $"org.opencontainers.image.created={DateTimeOffset.UtcNow:o}")
        //         .AppendSwitchQuoted("--label", $"org.opencontainers.image.revision={gitCommitSha}")
        //         .AppendSwitchQuoted("--label", $"org.opencontainers.image.version={version}")
        //         .AppendSwitchQuoted("--file", dockerfile.ToString())
        //         .Append(".")
        //         .RenderSafe());

        // If you'd rather not use buildx, then you can uncomment these lines instead.
        StartProcess(
            "docker",
            new ProcessArgumentBuilder()
                .Append("build")
                .AppendSwitchQuoted("--tag", $"{tag}:{version}")
                .AppendSwitchQuoted("--build-arg", $"Configuration={configuration}")
                .AppendSwitchQuoted("--label", $"org.opencontainers.image.created={DateTimeOffset.UtcNow:o}")
                .AppendSwitchQuoted("--label", $"org.opencontainers.image.revision={gitCommitSha}")
                .AppendSwitchQuoted("--label", $"org.opencontainers.image.version={version}")
                .AppendSwitchQuoted("--file", dockerfile.ToString())
                .Append(dockerfile.GetDirectory().ToString())
                .RenderSafe());
        if (push)
        {
            StartProcess(
                "docker",
                new ProcessArgumentBuilder()
                    .AppendSwitchQuoted("push", $"{tag}:{version}")
                    .RenderSafe());
        }

        string GetVersion()
        {
            var directoryBuildPropsFilePath = GetFiles("Directory.Build.props").Single().ToString();
            var directoryBuildPropsDocument = System.Xml.Linq.XDocument.Load(directoryBuildPropsFilePath);
            var preReleasePhase = directoryBuildPropsDocument.Descendants("MinVerDefaultPreReleasePhase").Single().Value;

            StartProcess(
                "dotnet",
                new ProcessSettings()
                    .WithArguments(x => x
                        .Append("minver")
                        .AppendSwitch("--default-pre-release-phase", preReleasePhase))
                    .SetRedirectStandardOutput(true),
                    out var versionLines);
            return versionLines.LastOrDefault();
        }

        string GetGitCommitSha()
        {
            StartProcess(
                "git",
                new ProcessSettings()
                    .WithArguments(x => x.Append("rev-parse HEAD"))
                    .SetRedirectStandardOutput(true),
                out var shaLines);
            return shaLines.LastOrDefault();
        }
    });

Task("BuildDocker")
    .Description("Cleans, restores NuGet packages, builds the solution, runs unit tests and then builds a Docker image.")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("DockerBuild");
Task("PublishArtifacts")
   .Description("Cleans, restores NuGet packages, builds the solution, runs unit tests and then publishe artifacts.")
   .IsDependentOn("Build")
   .IsDependentOn("Test")
   .IsDependentOn("Publish");
Task("RunAll")
    .Description("Cleans, restores NuGet packages, builds the solution, runs unit tests and then run all.")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Run");

RunTarget(target);
