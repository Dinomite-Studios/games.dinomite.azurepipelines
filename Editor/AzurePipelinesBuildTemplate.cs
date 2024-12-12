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
    private const string OutputFileNameArgument = "-outputFileName";
    private const string OutputPathArgument = "-outputPath";
    private const string AndroidKeystoreNameArgument = "-keystoreName";
    private const string AndroidKeystorePassArgument = "-keystorePass";
    private const string AndroidKeystoreAliasNameArgument = "-keystoreAliasName";
    private const string AndroidKeystoreAliasPassArgument = "-keystoreAliasPass";
    private const string AndroidBuildAppBundleArgument = "-buildAppBundle";

    public static void PerformBuild()
    {
        if (!Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(OutputPathArgument, out var locationPathName) ||
            !Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(OutputFileNameArgument, out var outputFileName))
        {
            Debug.LogError($"BUILD FAILED: Required command line arguments {OutputPathArgument}, {OutputFileNameArgument} not set.");
            EditorApplication.Exit(1);
            return;
        }

#if UNITY_6000_0_OR_NEWER
        // If building using Unity 6 or above we check whether we are building using a build profile.
        // When building using a build profile there is some settings we want to leave untouched as it is
        // part of the build profile itself to define those settings. E.g. there may be a build profile
        // for Android that outputs a AAB instead of APK and another one producing an APK file. We do not have to worry
        // about it as the developer simply has to selected the right build profile depending on the wanted output.
        var activeBuildProfile = UnityEditor.Build.Profile.BuildProfile.GetActiveBuildProfile();
        if (activeBuildProfile != null && activeBuildProfile)
        {
            Debug.Log("BUILD INFO: Building using build profile flow.");
            BuildUsingBuildProfile(activeBuildProfile, locationPathName, outputFileName);
            return;
        }
#endif

        Debug.Log("BUILD INFO: Building using build target flow.");
        BuildUsingBuildTarget(locationPathName, outputFileName);
    }

    private static void BuildUsingBuildTarget(string locationPathName, string outputFileName)
    {
        CheckAndroidSettings(true);

        try
        {
            var editorConfiguredBuildScenes = EditorBuildSettings.scenes;
            var includedScenes = new List<string>();

            for (int i = 0; i < editorConfiguredBuildScenes.Length; i++)
            {
                if (editorConfiguredBuildScenes[i].enabled == false)
                {
                    continue;
                }

                includedScenes.Add(editorConfiguredBuildScenes[i].path);
            }

#if UNITY_2018_1_OR_NEWER
            BuildReport buildReport = default;
#else
            var buildReport = "ERROR";
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
                    Debug.LogError($"BUILD FAILED: {buildReport.summary.result}\n{buildReport.summary}");
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
        catch (Exception ex)
        {
            Debug.LogError("BUILD FAILED: " + ex.Message);
            EditorApplication.Exit(1);
        }
    }

#if UNITY_6000_0_OR_NEWER
    private static void BuildUsingBuildProfile(UnityEditor.Build.Profile.BuildProfile buildProfile, string locationPathName, string outputFileName)
    {
        CheckAndroidSettings(false);

        try
        {
            var buildReport = BuildPipeline.BuildPlayer(new BuildPlayerWithProfileOptions
            {
                buildProfile = buildProfile,
                locationPathName = Path.Combine(locationPathName, GetBuildTargetOutputFileNameAndExtension(outputFileName)),
                options = BuildOptions.None
            });

            switch (buildReport.summary.result)
            {
                case BuildResult.Succeeded:
                    EditorApplication.Exit(0);
                    break;
                case BuildResult.Unknown:
                case BuildResult.Failed:
                case BuildResult.Cancelled:
                default:
                    Debug.LogError($"BUILD FAILED: {buildReport.summary.result}\n{buildReport.summary}");
                    EditorApplication.Exit(1);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("BUILD FAILED: " + ex.Message);
            EditorApplication.Exit(1);
        }
    }
#endif

    private static void CheckAndroidSettings(bool checkBuildAppBundle)
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            // When building for Android, check for custom keystore signing credentials and apply them prior to building the project.
            if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(AndroidKeystoreNameArgument, out var androidKeystoreName) &&
            Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(AndroidKeystorePassArgument, out var androidKeystorePass) &&
            Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(AndroidKeystoreAliasNameArgument, out var androidKeystoreAliasName))
            {
#if UNITY_2019_1_OR_NEWER
                PlayerSettings.Android.useCustomKeystore = true;
#endif
                PlayerSettings.Android.keystoreName = androidKeystoreName;
                PlayerSettings.Android.keystorePass = androidKeystorePass;
                PlayerSettings.Android.keyaliasName = androidKeystoreAliasName;
                PlayerSettings.Android.keyaliasPass = Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(AndroidKeystoreAliasPassArgument, out var androidKeystoreAliasPass) ?
                    androidKeystoreAliasPass :
                    androidKeystorePass;
            }
            else
            {
                // If no credentials passed, we assume the user wants to build using a development keystore auto-generated by Unity.
                // We clear the custom keystore flag here in case developers have checked in a keystore config to the repository, which
                // would cause the build to fail.
#if UNITY_2019_1_OR_NEWER
                PlayerSettings.Android.useCustomKeystore = false;
#endif
            }

            if (checkBuildAppBundle)
            {
                // When building for Android, we might want to build an .aab for the Play Store instead of an .apk file.
                EditorUserBuildSettings.buildAppBundle = Dinomite.AzurePipelines.Utilities.CommandLineArgumentExists(AndroidBuildAppBundleArgument);
            }
        }
    }

    private static string GetBuildTargetOutputFileNameAndExtension(string outputFileName)
    {
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                return string.Format("{0}.{1}", outputFileName, EditorUserBuildSettings.buildAppBundle ? "aab" : "apk");
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
#if !UNITY_2019_2_OR_NEWER
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneLinuxUniversal:
#endif
            case BuildTarget.WebGL:
            case BuildTarget.WSAPlayer:
            case BuildTarget.StandaloneLinux64:
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