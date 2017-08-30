using UnityEngine;

namespace Assets
{
    public static class ABConfig
    {
        /// <summary>
        /// 运行平台
        /// </summary>
        public const string PLATFORM =
#if UNITY_STANDALONE
            "Windows";
#elif UNITY_ANDROID
            "Android";
#else
            "";
#endif

        public const string KEY_SERVER = "Server";
        public const string SERVER_URL = "http://172.16.33.116:8088";

        public const string KEY_VERSION = "Version";
        public const int VERSION = 1;

        public const string KEY_MANIFEST = "Manifest";
        /// <summary>
        /// Manifest文件AB包名，这里通常与平台名相同
        /// </summary>
        public const string MANIFEST_NAME = PLATFORM;

        public const string KEY_ASSETBUNDLES = "AssetBundles";
        public const string KEY_ASSETS = "Assets";

        /// <summary>
        /// 资源清单文件
        /// </summary>
        public const string NAME_ASSETSMAP = "AssetsMap.json";
#if UNITY_EDITOR
        public const string LOCAL_PATH = "E:/GF47_Work/Projects/K12/TestAssetBundles";
#endif

        /// <summary>
        /// StreamingAssetPath的同步读取方式，使用AssetBundle.LoadfromFile加载
        /// </summary>
        public static string AssetbundleRoot_Streaming_AsFile =
#if UNITY_EDITOR
            Application.streamingAssetsPath;
#elif UNITY_STANDALONE
            Application.streamingAssetsPath;
#elif UNITY_ANDROID
            Application.streamingAssetsPath;
#else
            "Application.streamingAssetsPath";
#endif

        /// <summary>
        /// StreamingAssetPath的异步读取方式，即使用www加载
        /// </summary>
        public static string AssetbundleRoot_Streaming_AsWWW =
#if UNITY_EDITOR
             "file://" + Application.streamingAssetsPath;
#elif UNITY_STANDALONE
             "file://" + Application.streamingAssetsPath;
#elif UNITY_ANDROID
             Application.streamingAssetsPath;
            // "jar:file://" + Application.streamingAssetsPath; // 狗屎，网上说的都是要加jar，加个鸡毛
#else
            "file://" + "Application.streamingAssetsPath";
#endif

        /// <summary>
        /// 外部文件夹，热更新的文件
        /// </summary>
        public static string AssetbundleRoot_Hotfix =
#if UNITY_STANDALONE 
            Application.persistentDataPath;
#elif UNITY_ANDROID
            Application.persistentDataPath;

#else
            Application.persistentDataPath;
#endif
    }
}
