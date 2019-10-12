using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class QAssetBundleSample : MonoBehaviour
{
    //根目录的文件夹产生的AssetBundles文件地址
    private string rootAssetBundle = @"https://github.com/onelei/QAssetBundle/AssetBundles/Win64/AssetBundles/AssetBundles";
    //根目录地址
    private string assetBundleRoot = @"https://github.com/onelei/QAssetBundle/AssetBundles/Win64/AssetBundles/";

    public Button Button_Load;
    public Button Button_Load2;
    public Button Button_WebLoad;

    public Image Image_BackGround;

    private void Awake()
    {
        Button_Load.onClick.AddListener(OnClickLoad);
        Button_Load2.onClick.AddListener(OnClickLoad2);
        Button_WebLoad.onClick.AddListener(OnClickWebLoad);
    }

    void OnClickLoad()
    {
        Image_BackGround.overrideSprite = QAssetBundleManager.LoadResource<Sprite>("UI_1002", "uibackground");
    }

    void OnClickLoad2()
    {
        Image_BackGround.overrideSprite = QAssetBundleManager.LoadResource<Sprite>("UI_1003", "uibackground");
    }

    void OnClickWebLoad()
    {
        StartCoroutine(DownloadAssetBundles());
    }

    /// <summary>
    /// 下载根目录AssetBundle文件
    /// </summary>
    /// <returns></returns>
    IEnumerator DownloadAssetBundles()
    {
        //创建一个获取AssetBundle 文件的web 请求.
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(rootAssetBundle);

        //发送web请求
        yield return request.SendWebRequest();

        //从web获取内容，返回一个AssetBundle类型的数据
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);

        //获取AssetBundless.manifest的manifest文件
        AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundles");

        //获取AssetBundless.manifest文件中的所有AssetBundle的名称信息
        string[] assets = manifest.GetAllAssetBundles();
        for (int i = 0; i < assets.Length; i++)
        {
            Debug.Log(assetBundleRoot + assets[i]);
            //开启协程
            StartCoroutine(DownloadAssetBundleAndSave(assetBundleRoot + assets[i]));
        }
    }

    /// <summary>
    /// 下载AssetBundle文件并保存在本地
    /// </summary>
    /// <returns></returns>
    IEnumerator DownloadAssetBundleAndSave(string url)
    {
        //通过WWW 类创建一个web 请求，参数填写AssetBundle 的url 下载地址.
        WWW www = new WWW(url);

        //将对象作为数据返回，这个www 对象就是请求(下载）来的数据
        yield return www;

        //www.isDone,一个属性，表示下载状态是否完毕
        if (www.isDone)
        {
            ////使用IO技术将www对象存储到本地.
            SaveAssetBundle(Path.GetFileName(url), www.bytes, www.bytes.Length);
        }
    }

    /// <summary>
    /// 保存AssetBundle文件到本地
    /// </summary>
    private void SaveAssetBundle(string fileName, byte[] bytes, int count)
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "//" + fileName);
        FileStream fs = fileInfo.Create();

        //fs.Write(字节数组, 开始位置, 数据长度);
        fs.Write(bytes, 0, count);
        fs.Flush();     //文件写入存储到硬盘
        fs.Close();     //关闭文件流对象
        fs.Dispose();   //销毁文件对象
    }
}
