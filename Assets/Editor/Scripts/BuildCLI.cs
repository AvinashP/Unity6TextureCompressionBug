using System.Linq;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEngine;

namespace Editor.Scripts
{
    public static class BuildCLI
    {
        [MenuItem("Tools/Build")]
        public static void Create()
        {
            var selectedTarget = EditorUserBuildSettings.activeBuildTarget;

            var buildPlayerOptions = new BuildPlayerOptions()
            {
                targetGroup = BuildPipeline.GetBuildTargetGroup(selectedTarget),
                scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
                target = selectedTarget,
                locationPathName = "./Builds/TextureCompressionTest.apk",
                options = BuildOptions.CompressWithLz4,
                subtarget = (int) StandaloneBuildSubtarget.Player
            };
            
            if (!BuildPipeline.IsBuildTargetSupported(buildPlayerOptions.targetGroup, buildPlayerOptions.target))
                throw new BuildPlayerWindow.BuildMethodException("Build target is not supported.");
            if (EditorUserBuildSettings.activeBuildTarget != buildPlayerOptions.target || EditorUserBuildSettings.selectedBuildTargetGroup != buildPlayerOptions.targetGroup)
            {
                if (!EditorUserBuildSettings.SwitchActiveBuildTarget(buildPlayerOptions.targetGroup, buildPlayerOptions.target))
                    throw new BuildPlayerWindow.BuildMethodException($"Could not switch to build target '{buildPlayerOptions.target}', '{buildPlayerOptions.targetGroup}'.");
            }

            // Attempt to override texture compression format to ASTC
            UpdateBuildProfile();
           
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            Debug.Log(buildReport.summary);
        }
        
        public static void UpdateBuildProfile()
        {
            PlayerSettings.Android.textureCompressionFormats = new[] { TextureCompressionFormat.ASTC };
            EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ASTC;
            Debug.Log($"Active build profile is: {BuildProfile.GetActiveBuildProfile()?.name}");
        }
    }
}