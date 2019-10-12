using System.IO;
using UnityEditor;

/// <summary>
/// AssetBundle打包脚本Editor
/// </summary>
public class QAssetBundleEditor
{
    static string OUT_PATH_WIN64 = "AssetBundles/Win64/AssetBundles";
    static string OUT_PATH_IOS = "AssetBundles/IOS/AssetBundles";
    static string OUT_PATH_Android = "AssetBundles/Android/AssetBundles";

    /// <summary>
    /// BuildWin64
    /// </summary>
    [MenuItem("AssetBundle/BuildWin64")]
    public static void BuildAssetBundle_Win64()
    {
        BuildAssetBundles(OUT_PATH_WIN64, BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// BuildWin64
    /// </summary>
    [MenuItem("AssetBundle/BuildIOS")]
    public static void BuildAssetBundle_IOS()
    {
        BuildAssetBundles(OUT_PATH_IOS, BuildTarget.iOS);
    }

    /// <summary>
    /// BuildWin64
    /// </summary>
    [MenuItem("AssetBundle/BuildAndroid")]
    public static void BuildAssetBundle_Android()
    {
        BuildAssetBundles(OUT_PATH_Android, BuildTarget.Android);
    }

    public static void BuildAssetBundles(string outPath, BuildTarget buildTarget)
    {
        if (Directory.Exists(outPath))
        {
            Directory.Delete(outPath, true);
        }
        Directory.CreateDirectory(outPath);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.UncompressedAssetBundle, buildTarget);

        AssetDatabase.Refresh();
    }
}
