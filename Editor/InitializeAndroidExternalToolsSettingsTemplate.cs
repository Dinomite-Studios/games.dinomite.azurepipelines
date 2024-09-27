using UnityEditor;
using UnityEditor.Android;

public static class InitializeAndroidExternalToolsSettings
{
    public static void SetupAndroidToolsFromCmd()
    {
        const string androidJdkPathArgumentName = "overrideAndroidJdkPath";
        const string androidSdkPathArgumentName = "overrideAndroidSdkPath";
        const string androidNdkPathArgumentName = "overrideAndroidNdkPath";
        const string androidGradlePathArgumentName = "overrideAndroidGradlePath";

        UnityEngine.Debug.LogFormat("Current JDK path: {0}", AndroidExternalToolsSettings.jdkRootPath);
        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidJdkPathArgumentName, out var androidJdkPath))
        {
            UnityEngine.Debug.LogFormat("Override JDK path: {0}", androidJdkPath);
            AndroidExternalToolsSettings.jdkRootPath = androidJdkPath;
        }

        UnityEngine.Debug.LogFormat("Current SDK path: {0}", AndroidExternalToolsSettings.sdkRootPath);
        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidSdkPathArgumentName, out var androidSdkPath))
        {
            UnityEngine.Debug.LogFormat("Override SDK path: {0}", androidSdkPath);
            AndroidExternalToolsSettings.sdkRootPath = androidSdkPath;
        }

        UnityEngine.Debug.LogFormat("Current NDK path: {0}", AndroidExternalToolsSettings.ndkRootPath);
        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidNdkPathArgumentName, out var androidNdkPath))
        {
            UnityEngine.Debug.LogFormat("Override NDK path: {0}", androidNdkPath);
            AndroidExternalToolsSettings.ndkRootPath = androidNdkPath;
        }

        UnityEngine.Debug.LogFormat("Current gradle path: {0}", AndroidExternalToolsSettings.gradlePath);
        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidGradlePathArgumentName, out var androidGradlePath))
        {
            UnityEngine.Debug.LogFormat("Override gradle path: {0}", androidGradlePath);
            AndroidExternalToolsSettings.gradlePath = androidGradlePath;
        }

        EditorApplication.Exit(0);
    }
}
