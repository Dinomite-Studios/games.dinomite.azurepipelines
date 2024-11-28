using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

public static class AzurePipelinesBuild
{
    private const string OutputFileNameArgument = "outputFileName";
    private const string OutputPathArgument = "outputPath";

    public static void PerformBuild()
    {
        if (!Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(OutputPathArgument, out var locationPathName) ||
            !Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(OutputFileNameArgument, out var outputFileName))
        {
            locationPathName = "Build";
            outputFileName = "drop";
            // EditorApplication.Exit(1);
            // return;
        }

        try
        {
            EditorBuildSettingsScene[] editorConfiguredBuildScenes = EditorBuildSettings.scenes;
            var includedScenes = new List<string>();

            for (int i = 0; i < editorConfiguredBuildScenes.Length; i++)
            {
                if (editorConfiguredBuildScenes[i].enabled == false)
                    continue;

                includedScenes.Add(editorConfiguredBuildScenes[i].path);
            }

#if UNITY_2018_1_OR_NEWER
            BuildReport buildReport = default(BuildReport);
#else
            string buildReport = "ERROR";
#endif

            buildReport = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = includedScenes.ToArray(),
                target = EditorUserBuildSettings.activeBuildTarget,
                locationPathName = Path.Combine(locationPathName, GetBuildTargetOutputFileNameAndExtension(outputFileName)),
                targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup,
                options = BuildOptions.None
            });

#if UNITY_2018_1_OR_NEWER
            switch (buildReport.summary.result)
            {
                case BuildResult.Succeeded:
                    EditorApplication.Exit(0);
                    break;
                case BuildResult.Unknown:
                case BuildResult.Failed:
                case BuildResult.Cancelled:
                default:
                    EditorApplication.Exit(1);
                    break;
            }
#else
            if (buildReport.StartsWith("Error"))
            {
                EditorApplication.Exit(1);
            }
            else
            {
                EditorApplication.Exit(0);
            }
#endif
        }
        catch (Exception ex) when (ex is Exception)
        {
            Debug.Log("BUILD FAILED: " + ex.Message);
            EditorApplication.Exit(1);
        }
    }

    private static string GetBuildTargetOutputFileNameAndExtension(string outputFileName)
    {
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                return string.Format("{0}.apk", outputFileName);
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneWindows:
                return string.Format("{0}.exe", outputFileName);
#if UNITY_2018_1_OR_NEWER
            case BuildTarget.StandaloneOSX:
#endif
#if !UNITY_2017_3_OR_NEWER
                    case BuildTarget.StandaloneOSXIntel:
                    case BuildTarget.StandaloneOSXIntel64:
#endif
                return string.Format("{0}.app", outputFileName);
            case BuildTarget.iOS:
            case BuildTarget.tvOS:
            case BuildTarget.StandaloneLinux:
            case BuildTarget.WebGL:
            case BuildTarget.WSAPlayer:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneLinuxUniversal:
#if !UNITY_2018_3_OR_NEWER
                    case BuildTarget.PSP2:    
#endif
            case BuildTarget.PS4:
            case BuildTarget.XboxOne:
#if !UNITY_2017_3_OR_NEWER
                    case BuildTarget.SamsungTV:
#endif
#if !UNITY_2018_1_OR_NEWER
                    case BuildTarget.N3DS:
                    case BuildTarget.WiiU:
#endif
            case BuildTarget.Switch:
            case BuildTarget.NoTarget:
            default:
                return outputFileName;
        }
    }
}