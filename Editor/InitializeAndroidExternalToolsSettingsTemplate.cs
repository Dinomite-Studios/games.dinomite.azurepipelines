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

        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidJdkPathArgumentName, out var androidJdkPath))
        {
            AndroidExternalToolsSettings.jdkRootPath = androidJdkPath;
        }

        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidSdkPathArgumentName, out var androidSdkPath))
        {
            AndroidExternalToolsSettings.sdkRootPath = androidSdkPath;
        }

        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidNdkPathArgumentName, out var androidNdkPath))
        {
            AndroidExternalToolsSettings.ndkRootPath = androidNdkPath;
        }

        if (Dinomite.AzurePipelines.Utilities.TryGetCommandLineArgumentValue(androidGradlePathArgumentName, out var androidGradlePath))
        {
            AndroidExternalToolsSettings.gradlePath = androidGradlePath;
        }

        EditorApplication.Exit(0);
    }
}
