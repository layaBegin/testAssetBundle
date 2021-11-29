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
        //��ͬƽ̨��StreamingAssets��·���ǲ�ͬ�ģ�������Ҫע��һ�¡�  
        Debug.Log("==pathUrl:" + PathURL);
        single = AssetBundle.LoadFromFile(PathURL + "AssetBundle/AssetBundle");
        manifest = single.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //����Ԥ����----------------
        AssetBundle ab = AssetBundle.LoadFromFile(PathURL + "AssetBundle/pfb");
        //������ȼ�����������ɵ�����ᶪʧ����
        LoadDep("pfb");
        GameObject cubePrefab = ab.LoadAsset<GameObject>("Cube");
        Instantiate(cubePrefab);
        GameObject imagePrefab = ab.LoadAsset<GameObject>("Image");
        Instantiate(imagePrefab,pos1);

        //���س���
        LoadDep("sce");
        AssetBundle sce = AssetBundle.LoadFromFile(PathURL + "AssetBundle/sce");
        SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);//����ģʽ��׷�ӻ��滻��Ĭ���滻
        //�ڶ��ַ���
        //WWW download = WWW.LoadFromCacheOrDownload(PathURL + "AssetBundle/sce", 1);
        //var bundle = download.assetBundle;
        //SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
         
        

    }


    void LoadDep(string abName)
    {
        string[] deps = manifest.GetAllDependencies(abName);
        for (int i = 0; i < deps.Length; i++)
        {
            if (!list.Contains(deps[i]))//ֻ��û���ع��ż���
            {
                Debug.Log("=====��������:"+deps[i]);
                AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/" + deps[i]);
                list.Add(deps[i]);
                LoadDep(deps[i]);//�ݹ�������еĶ��������
            }
        }
    }
}
