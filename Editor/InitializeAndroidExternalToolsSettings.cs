using UnityEditor.Android;

namespace Dinomite.AzurePipelines
{
    public static class InitializeAndroidTools
    {
        public static void SetupAndroidToolsFromCmd()
        {
            const string androidJdkPathArgumentName = "overrideAndroidJdkPath";
            const string androidSdkPathArgumentName = "overrideAndroidSdkPath";
            const string androidNdkPathArgumentName = "overrideAndroidNdkPath";
            const string androidGradlePathArgumentName = "overrideAndroidGradlePath";

            if (Utilities.TryGetCommandLineArgumentValue(androidJdkPathArgumentName, out var androidJdkPath))
            {
                AndroidExternalToolsSettings.jdkRootPath = androidJdkPath;
            }

            if (Utilities.TryGetCommandLineArgumentValue(androidSdkPathArgumentName, out var androidSdkPath))
            {
                AndroidExternalToolsSettings.sdkRootPath = androidSdkPath;
            }

            if (Utilities.TryGetCommandLineArgumentValue(androidNdkPathArgumentName, out var androidNdkPath))
            {
                AndroidExternalToolsSettings.ndkRootPath = androidNdkPath;
            }

            if (Utilities.TryGetCommandLineArgumentValue(androidGradlePathArgumentName, out var androidGradlePath))
            {
                AndroidExternalToolsSettings.gradlePath = androidGradlePath;
            }
        }
    }
}
