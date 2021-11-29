using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ABManager : MonoBehaviour
{
    public Transform pos1;
    AssetBundle single;
    AssetBundleManifest manifest;
    
    List<string> list = new List<string>();

    private string PathURL;
    // Start is called before the first frame update
    private void Awake()
    {
        PathURL =
#if UNITY_ANDROID
        "jar:file://" + Application.dataPath + "!/assets/";  
#elif UNITY_IPHONE
        Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
         Application.dataPath + "/StreamingAssets/";
#else
                string.Empty;  
#endif
    }

    void Start()
    {
        //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。  
        Debug.Log("==pathUrl:" + PathURL);
        single = AssetBundle.LoadFromFile(PathURL + "AssetBundle/AssetBundle");
        manifest = single.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //加载预制体----------------
        AssetBundle ab = AssetBundle.LoadFromFile(PathURL + "AssetBundle/pfb");
        //如果不先加载依赖项，生成的物体会丢失依赖
        LoadDep("pfb");
        GameObject cubePrefab = ab.LoadAsset<GameObject>("Cube");
        Instantiate(cubePrefab);
        GameObject imagePrefab = ab.LoadAsset<GameObject>("Image");
        Instantiate(imagePrefab,pos1);

        //加载场景
        LoadDep("sce");
        AssetBundle sce = AssetBundle.LoadFromFile(PathURL + "AssetBundle/sce");
        SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);//场景模式有追加或替换，默认替换
        //第二种方法
        //WWW download = WWW.LoadFromCacheOrDownload(PathURL + "AssetBundle/sce", 1);
        //var bundle = download.assetBundle;
        //SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
         
        

    }


    void LoadDep(string abName)
    {
        string[] deps = manifest.GetAllDependencies(abName);
        for (int i = 0; i < deps.Length; i++)
        {
            if (!list.Contains(deps[i]))//只有没加载过才加载
            {
                Debug.Log("=====加载依赖:"+deps[i]);
                AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/" + deps[i]);
                list.Add(deps[i]);
                LoadDep(deps[i]);//递归加载所有的多层依赖想
            }
        }
    }
}
