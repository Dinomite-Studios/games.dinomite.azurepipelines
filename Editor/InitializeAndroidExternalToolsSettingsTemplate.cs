using UnityEditor;
using UnityEditor.Android;

public static class InitializeAndroidExternalToolsSettings
{
    public static void SetupAndroidToolsFromCmd()
    {
        UnityEngine.Debug.LogFormat("Current JDK path: {0}", AndroidExternalToolsSettings.jdkRootPath);
        AndroidExternalToolsSettings.jdkRootPath = null;

        UnityEngine.Debug.LogFormat("Current SDK path: {0}", AndroidExternalToolsSettings.sdkRootPath);
        AndroidExternalToolsSettings.sdkRootPath = null;

        UnityEngine.Debug.LogFormat("Current NDK path: {0}", AndroidExternalToolsSettings.ndkRootPath);
        AndroidExternalToolsSettings.ndkRootPath = null;

        UnityEngine.Debug.LogFormat("Current gradle path: {0}", AndroidExternalToolsSettings.gradlePath);
        AndroidExternalToolsSettings.gradlePath = null;

        EditorApplication.Exit(0);
    }
}
