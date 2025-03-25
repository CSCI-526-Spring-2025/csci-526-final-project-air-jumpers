using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;
using System.IO;
using System.Diagnostics;

public class BuildScript
{
    public static string tagPrefix = "beta";

    private static string repoRoot = Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;

    [MenuItem("Tools/Build And Push")]
    public static void BuildAndTag()
    {
        string branch = GetGitBranch().Trim();

        if (!branch.Equals("main", System.StringComparison.OrdinalIgnoreCase))
        {
            EditorUtility.DisplayDialog("Error", "Current branch is not main. Exiting.", "OK");
            return;
        }

        RunGitCommand("checkout build");
        RunGitCommand("pull origin build");

        AssetDatabase.Refresh();
        string currentVersion = PlayerSettings.bundleVersion;
        string newVersion = BumpVersion(currentVersion);

        RunGitCommand("checkout main");
        RunGitCommand("pull origin main");
        RunGitCommand($"tag -a {newVersion} -m \"Version {tagPrefix}-{newVersion}\"");
        RunGitCommand("push origin HEAD");
        RunGitCommand("push origin --tags");

        RunGitCommand("checkout build");
        RunGitCommand("merge main --no-edit");

        PlayerSettings.bundleVersion = newVersion;
        AssetDatabase.SaveAssets();

        BuildWebGL($"{repoRoot}/{tagPrefix}-build");

        RunGitCommand("add .");
        RunGitCommand($"commit -m \"Build version {tagPrefix}-{newVersion}\"");
        RunGitCommand($"tag -a {newVersion} -m \"Version {tagPrefix}-{newVersion}-build\"");
        RunGitCommand("push origin HEAD");
        RunGitCommand("push origin --tags");
    }


    private static void BuildWebGL(string outputPath)
    {
        string[] scenes = EditorBuildSettings.scenes
                                .Where(s => s.enabled)
                                .Select(s => s.path)
                                .ToArray();

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
            UnityEngine.Debug.Log("WebGL Build succeeded: " + summary.totalSize + " bytes");
        else
            UnityEngine.Debug.LogError("WebGL Build failed!");
    }

    private static string BumpVersion(string currentVersion)
    {
        string[] parts = currentVersion.Split('.');
        if (parts.Length < 3)
            return "1.0.1";
        int major = int.Parse(parts[0]);
        int minor = int.Parse(parts[1]);
        int patch = int.Parse(parts[2]);
        patch++;
        return $"{major}.{minor}.{patch}";
    }

    private static void RunGitCommand(string arguments)
    {
        ProcessStartInfo psi = new ProcessStartInfo("git", arguments)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = repoRoot
        };
        using (Process process = Process.Start(psi))
        {
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            UnityEngine.Debug.Log($"git {arguments}:\n{output}\n{error}");
        }
    }

    private static string GetGitBranch()
    {
        ProcessStartInfo psi = new ProcessStartInfo("git", "rev-parse --abbrev-ref HEAD")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = repoRoot
        };
        using (Process process = Process.Start(psi))
        {
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }
    }
}
