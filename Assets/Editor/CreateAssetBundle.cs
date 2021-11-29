using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateAssetBundle : Editor {

    [MenuItem("AssetBundle/Create")]
    static void CreateAB()
    {
        Debug.Log("streamingAssetsPath:" + Application.streamingAssetsPath);

        //注意：1，每个平台都要分开打包
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetBundle", 
            BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
    }

}
