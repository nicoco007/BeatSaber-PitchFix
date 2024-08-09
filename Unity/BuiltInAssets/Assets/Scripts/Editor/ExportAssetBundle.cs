using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportAssetBundle
{
    [MenuItem("Assets/Export Pitch Fix Asset Bundle", priority = 1100)]
    public static void BuildAssetBundle()
    {
        string resourcesPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..", "Source", "PitchFix", "Resources"));
        string targetPath = EditorUtility.SaveFilePanel("Export Pitch Fix Asset Bundle", resourcesPath, "Assets", string.Empty);

        if (string.IsNullOrEmpty(targetPath))
        {
            return;
        }

        AssetBundleBuild assetBundleBuild = new()
        {
            assetBundleName = "PitchFixAssets",
            assetNames = new[] {
                "Assets/PitchFix.mixer",
            },
        };

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new AssetBundleBuild[] { assetBundleBuild }, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);

        if (manifest == null)
        {
            EditorUtility.DisplayDialog("Failed to build asset bundle!", "Failed to build asset bundle! Check the console for details.", "OK");
            return;
        }

        string fileName = manifest.GetAllAssetBundles()[0];
        File.Copy(Path.Combine(Application.temporaryCachePath, fileName), targetPath, true);

        EditorUtility.DisplayDialog("Export Successful!", "Asset bundle exported successfully!", "OK");
    }
}
