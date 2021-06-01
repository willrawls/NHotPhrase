﻿using System.IO;
using System.Runtime.CompilerServices;
using McMaster.Extensions.CommandLineUtils;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace buildNHotPhrase
{
    [SuppressDefaultHelpOption]
    public class Build
    {
        static void Main(string[] args) =>
            CommandLineApplication.Execute<Build>(args);

        [Option("-h|-?|--help", "Show help message", CommandOptionType.NoValue)]
        public bool ShowHelp { get; } = false;

        [Option("-c|--configuration", "The configuration to build", CommandOptionType.SingleValue)]
        public string Configuration { get; } = "Release";

        public string[] RemainingArguments { get; } = null;

        public void OnExecute(CommandLineApplication app)
        {
            if (ShowHelp)
            {
                app.ShowHelp();
                app.Out.WriteLine("Bullseye help:");
                app.Out.WriteLine();
                RunTargetsAndExit(new[] { "-h" });
                return;
            }

            Directory.SetCurrentDirectory(GetSolutionDirectory());

            var artifactsDir = Path.GetFullPath("artifacts");
            var logsDir = Path.Combine(artifactsDir, "logs");
            var buildLogFile = Path.Combine(logsDir, "build.binlog");
            var packagesDir = Path.Combine(artifactsDir, "packages");

            var solutionFile = "NHotPhrase.sln";

            Target(
                "artifactDirectories",
                () =>
                {
                    Directory.CreateDirectory(artifactsDir);
                    Directory.CreateDirectory(logsDir);
                    Directory.CreateDirectory(packagesDir);
                });

            Target(
                "build",
                DependsOn("artifactDirectories"),
                () => Run(
                    "dotnet",
                    $"build -c \"{Configuration}\" /bl:\"{buildLogFile}\" \"{solutionFile}\""));

            Target(
                "pack",
                DependsOn("artifactDirectories", "build"),
                () => Run(
                    "dotnet",
                    $"pack -c \"{Configuration}\" --no-build -o \"{packagesDir}\""));

            Target("default", DependsOn("pack"));

            RunTargetsWithoutExiting(RemainingArguments);
        }

        public static string GetSolutionDirectory() =>
            Path.GetFullPath(Path.Combine(GetScriptDirectory(), @"..\.."));

        public static string GetScriptDirectory([CallerFilePath] string filename = null) => Path.GetDirectoryName(filename);
    }
}