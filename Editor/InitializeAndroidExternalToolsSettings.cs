using UnityEditor;
using UnityEditor.Android;

namespace Dinomite.AzurePipelines
{
    public static class InitializeAndroidTools
    {
        [InitializeOnLoadMethod]
        public static void Initialze()
        {
            const string androidJdkPathArgumentName = "overrideAndroidJdkPath";
            const string androidSdkPathArgumentName = "overrideAndroidSdkPath";
            const string androidNdkPathArgumentName = "overrideAndroidNdkPath";
            const string androidGradlePathArgumentName = "overrideAndroidGradlePath";

            if (string.IsNullOrEmpty(AndroidExternalToolsSettings.jdkRootPath) &&
                Utilities.TryGetCommandLineArgumentValue(androidJdkPathArgumentName, out var androidJdkPath))
            {
                AndroidExternalToolsSettings.jdkRootPath = androidJdkPath;
            }

            if (string.IsNullOrEmpty(AndroidExternalToolsSettings.sdkRootPath) &&
                Utilities.TryGetCommandLineArgumentValue(androidSdkPathArgumentName, out var androidSdkPath))
            {
                AndroidExternalToolsSettings.sdkRootPath = androidSdkPath;
            }

            if (string.IsNullOrEmpty(AndroidExternalToolsSettings.ndkRootPath) &&
                Utilities.TryGetCommandLineArgumentValue(androidNdkPathArgumentName, out var androidNdkPath))
            {
                AndroidExternalToolsSettings.ndkRootPath = androidNdkPath;
            }

            if (string.IsNullOrEmpty(AndroidExternalToolsSettings.gradlePath) &&
                Utilities.TryGetCommandLineArgumentValue(androidGradlePathArgumentName, out var androidGradlePath))
            {
                AndroidExternalToolsSettings.gradlePath = androidGradlePath;
            }
        }
    }
}
