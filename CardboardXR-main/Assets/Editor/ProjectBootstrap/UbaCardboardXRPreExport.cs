using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProjectBootstrap
{
    // Configure CardboardXR's XR providers and generate its complete scene list
    // before Unity Build Automation exports the iOS player.
    public static class UbaCardboardXRPreExport
    {
        // Set this exact method in Build Automation > Configuration > Advanced Settings
        // > Pre-export method.
        public static void PreExport()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                throw new System.InvalidOperationException(
                    "[CardboardXR] This cloud pre-export hook is intended for an iOS build.");
            }

            ProjectSetup.ApplyForCloudBuild();

            // Creates the generated Core scene and any missing demo scenes.
            var scenes = StereoBuilder.BuildScenes();

            // Make UBA's normal iOS export use the exact scene order created by CardboardXR.
            EditorBuildSettings.scenes = scenes
                .Select(path => new EditorBuildSettingsScene(path, true))
                .ToArray();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[CardboardXR] Cloud pre-export setup completed. Scenes: " +
                      string.Join(", ", scenes));
        }
    }
}
