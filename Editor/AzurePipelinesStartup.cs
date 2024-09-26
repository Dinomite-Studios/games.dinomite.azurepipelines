using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dinomite.AzurePipelines
{
    [InitializeOnLoad]
    public static class AzurePipelinesStartup
    {
        private const string InitializeAndroidExternalToolsSettingsTemplateFileName = "InitializeAndroidExternalToolsSettingsTemplate.cs";
        private const string InitializeAndroidExternalToolsSettingsFileName = "InitializeAndroidExternalToolsSettings.cs";
        private const string AzurePipelinesBuildTemplateFileName = "AzurePipelinesBuildTemplate.cs";
        private const string AzurePipelinesBuildFileName = "AzurePipelinesBuild.cs";
        private const string packageIdentifer = "games.dinomite.azurepipelines";

        static AzurePipelinesStartup()
        {
#pragma warning disable UNT0031
            var didModifyAssets = false;

            var pipelineEntryPointsFolderPath = GetEntryPointsRootFolder();
            if (!Directory.Exists(pipelineEntryPointsFolderPath))
            {
                Directory.CreateDirectory(pipelineEntryPointsFolderPath);
                didModifyAssets = true;
            }

            var androidToolsSettingsFilePath = Path.Combine(pipelineEntryPointsFolderPath, InitializeAndroidExternalToolsSettingsFileName);
            if (!File.Exists(androidToolsSettingsFilePath))
            {
                var templatePath = Path.Combine($"Packages", packageIdentifer, "Editor", InitializeAndroidExternalToolsSettingsTemplateFileName);
                File.Copy(templatePath, androidToolsSettingsFilePath);
                didModifyAssets = true;
            }

            var azurePipelinesBuildFilePath = Path.Combine(pipelineEntryPointsFolderPath, AzurePipelinesBuildFileName);
            if (!File.Exists(azurePipelinesBuildFilePath))
            {
                var templatePath = Path.Combine($"Packages", packageIdentifer, "Editor", AzurePipelinesBuildTemplateFileName);
                File.Copy(templatePath, azurePipelinesBuildFilePath);
                didModifyAssets = true;
            }

            if (didModifyAssets)
            {
                AssetDatabase.Refresh();
            }
#pragma warning restore UNT0031
        }

        private static string GetEntryPointsRootFolder() => Path.Combine(Application.dataPath, "Dinomite.AzurePipelines", "Editor");
    }
}
