/***************************************************************
 * @File Name       : BuildAB
 * @Author          : GF47
 * @Description     : 打包命令

 * 先选中需要打包的资源
 * 设置所在包名，可以选择将依赖的资源一起设置
 * ! 目录以[@]开头的，视为目录所有内容都打到以这个目录为名的包里
 * ! 目录或资源名不能出现[空格、#]等字符，在3ds Max或Maya生成的资源名经常会有这些字符，请自行处理
 * 执行打包命令，在指定平台的目录中可以看到生成的包文件
 * 
 * @Date            : 2017/7/19/星期三 14:22:10
 * @Edit            : none
 **************************************************************/

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Assets;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using SimpleJSON;

public class BuildAB
{
    public const string CHAR_COLLECT_SUBASSETS_TO_SINGLE_ASSETBUNDLE = "@";
    public const int GUID_SUB_LENGTH = 5;

    public const string ASSETBUNDLES_ROOT_DIRECTORY = "AssetBundles";

    private static readonly Regex Regex = new Regex(@"[\s#\.]+");

    /// <summary>
    /// 设置包名
    /// </summary>
    [MenuItem("AssetBundles/Set AssetBundle Name")]
    static void SetABName()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (objects == null || objects.Length < 1)
        {
            Debug.LogWarning("请选择打包资源或者目录");
            return;
        }
        foreach (Object obj in objects) { SetSingleABName(obj); }
    }

    /// <summary>
    /// 设置包名，连同依赖资源一起
    /// </summary>
    [MenuItem("AssetBundles/Set AssetBundle Name With Dependencies")]
    static void SetABNameWithDependencies()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (objects == null || objects.Length < 1)
        {
            Debug.LogWarning("请选择打包资源或者目录");
            return;
        }

        foreach (Object obj in objects)
        {
            string[] dependencies = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(obj));
            foreach (string dependency in dependencies)
            {
                SetSingleABName(AssetDatabase.LoadMainAssetAtPath(dependency));
            }
        }
    }

    private static void SetSingleABName(Object obj)
    {
        if (ProjectWindowUtil.IsFolder(obj.GetInstanceID())) return;
        if (obj is MonoScript) return;

        string assetbundleName, guid;
        string path = AssetDatabase.GetAssetPath(obj);

        if (path.Contains(CHAR_COLLECT_SUBASSETS_TO_SINGLE_ASSETBUNDLE)) // 带有指定字符开头的目录下的资源被打成一个包，包名为目录名
        {
            int charID = path.LastIndexOf(CHAR_COLLECT_SUBASSETS_TO_SINGLE_ASSETBUNDLE, StringComparison.Ordinal);
            string right = path.Substring(charID);
            int slashID = right.IndexOf('/');

            int length = charID;
            length += slashID > -1 ? slashID : right.Length;

            guid = AssetDatabase.AssetPathToGUID(path.Substring(0, length)).Substring(0, GUID_SUB_LENGTH);

            assetbundleName = path.Substring(7, length - 7);
        }
        else // 单文件打包
        {
            guid = AssetDatabase.AssetPathToGUID(path).Substring(0, GUID_SUB_LENGTH);

            assetbundleName = path.Substring(7);
        }

        assetbundleName = string.Format("{0}_{1}", assetbundleName, guid); // 添加guid前几位，避免命名冲突
        assetbundleName = Regex.Replace(assetbundleName, "_"); // 去除路径中的非法字符
        AssetImporter importer = AssetImporter.GetAtPath(path);
        importer.assetBundleName = assetbundleName;
    }

    /// <summary>
    /// 忽略掉选中的资源
    /// </summary>
    [MenuItem("AssetBundles/Ignore")]
    static void IgnoreSelected()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in objects)
        {
            IgnoreSingleObjec(obj);
        }

        AssetDatabase.RemoveUnusedAssetBundleNames();
    }

    /// <summary>
    /// 忽略掉选中的资源，连同依赖资源一起
    /// </summary>
    [MenuItem("AssetBundles/Ignore With Dependencies")]
    static void IgnoreSelectedWithDependencies()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in objects)
        {
            string[] dependencies = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(obj));
            foreach (string dependency in dependencies)
            {
                IgnoreSingleObjec(AssetDatabase.LoadMainAssetAtPath(dependency));
            }
        }
        AssetDatabase.RemoveUnusedAssetBundleNames();
    }

    private static void IgnoreSingleObjec(Object obj)
    {
        if (obj is MonoScript) return;
        AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj));
        importer.assetBundleName = string.Empty;
    }

    /// <summary>
    /// 创建安卓平台的AB包
    /// </summary>
    [MenuItem("AssetBundles/Build Android")]
    static void BuildAll_Android()
    {
        Caching.CleanCache();

        string[] assetbundleNames = AssetDatabase.GetAllAssetBundleNames();
        AssetBundleBuild[] abBuilds = new AssetBundleBuild[assetbundleNames.Length];
        for (int i = 0; i < abBuilds.Length; i++)
        {
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = assetbundleNames[i];
            build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(build.assetBundleName);
            build.assetBundleVariant = string.Empty; // TODO 添加LOD时会需要，或者根据机型性能区分

            abBuilds[i] = build;
        }

        string outputPath = CreateABDirectory(Platform.Android, Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/" + ASSETBUNDLES_ROOT_DIRECTORY);
        BuildPipeline.BuildAssetBundles(outputPath, abBuilds, BuildAssetBundleOptions.None, BuildTarget.Android);
        AssetDatabase.Refresh();

        CopyAssetsMapFileTo(outputPath);
    }

    [MenuItem("AssetBundles/Build Windows")]
    static void BuildAll_Windows()
    {
        Caching.CleanCache();

        string[] assetbundleNames = AssetDatabase.GetAllAssetBundleNames();
        AssetBundleBuild[] abBuilds = new AssetBundleBuild[assetbundleNames.Length];
        for (int i = 0; i < abBuilds.Length; i++)
        {
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = assetbundleNames[i];
            build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(build.assetBundleName);
            build.assetBundleVariant = string.Empty; // TODO 添加LOD时会需要，或者根据机型性能区分

            abBuilds[i] = build;
        }

        string outputPath = CreateABDirectory(Platform.Windows, Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/" + ASSETBUNDLES_ROOT_DIRECTORY);
        BuildPipeline.BuildAssetBundles(outputPath, abBuilds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();

        CopyAssetsMapFileTo(outputPath);
    }

    /// <summary>
    /// 创建指定平台的输出目录
    /// </summary>
    /// <param name="platform">具体的平台</param>
    /// <param name="exportDirectory">输出目录，为空时则认为是输出到 [StreamingAssets] 目录下的指定平台目录</param>
    /// <returns>输出目录</returns>
    private static string CreateABDirectory(Platform platform, string exportDirectory = null)
    {
        string platformDir = string.Empty;
        switch (platform)
        {
            case Platform.Windows:
                platformDir = "Windows";
                break;
            case Platform.Mac:
                platformDir = "Mac";
                break;
            case Platform.Android:
                platformDir = "Android";
                break;
            case Platform.iOS:
                platformDir = "iOS";
                break;
            default:
                break;
        }

        bool isExternal = !string.IsNullOrEmpty(exportDirectory);
        if (isExternal)
        {
            exportDirectory = string.Format("{0}/{1}", exportDirectory, platformDir);
            if (!Directory.Exists(exportDirectory)) { Directory.CreateDirectory(exportDirectory); }
        }
        else
        {
            if (!Directory.Exists(Application.streamingAssetsPath)) { Directory.CreateDirectory(Application.streamingAssetsPath); }
            exportDirectory = string.Format("{0}/{1}", Application.streamingAssetsPath, platformDir);
            if (!Directory.Exists(exportDirectory)) { Directory.CreateDirectory(exportDirectory); }
        }

        return exportDirectory;
    }

    [MenuItem("AssetBundles/Create Assets Map")]
    private static string CreateAssetsMap()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();

        string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

        JSONObject jsonAssetBundleNames = new JSONObject();
        for (int i = 0; i < assetBundleNames.Length; i++)
        {
            jsonAssetBundleNames.Add(assetBundleNames[i], i);
        }

        JSONObject jsonAssets = new JSONObject();
        for (int i = 0; i < assetBundleNames.Length; i++)
        {
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleNames[i]);
            for (int j = 0; j < assetPaths.Length; j++)
            {
                jsonAssets.Add(assetPaths[j], i);
            }
        }

        JSONObject jsonObject = new JSONObject
        {
            {ABConfig.KEY_SERVER, ABConfig.SERVER_URL},
            {ABConfig.KEY_VERSION, ABConfig.VERSION},
            {ABConfig.KEY_ASSETBUNDLES, jsonAssetBundleNames},
            {ABConfig.KEY_ASSETS, jsonAssets}
        };

        string json = jsonObject.ToString();
        string assetsMapFullName = Application.streamingAssetsPath + "/" + ABConfig.NAME_ASSETSMAP;

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        StreamWriter sw = new StreamWriter(assetsMapFullName, false, Encoding.UTF8);
        try
        {
            sw.Write(json);
            sw.Flush();
            sw.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            sw.Close();
        }

        AssetDatabase.Refresh();

        return assetsMapFullName;
    }

    private static void CopyAssetsMapFileTo(string outPutPath)
    {
        string fileName = CreateAssetsMap();
        if (File.Exists(fileName))
        {
            File.Copy(fileName, outPutPath + "/" + ABConfig.NAME_ASSETSMAP, true);
        }
    }
}
