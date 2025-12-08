using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BundleMan
{
    private static AssetBundle mapAssetBundle;
    private static AssetBundle GetTiledAssetBundle()
    {
        #if UNITY_IOS
        string tileName = $"iOS/tilemap";
        #else
        string tileName = $"Android/tilemap";
        #endif
        
        AssetBundle mapBundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, tileName));
        
        if (mapBundle == null)
        {
            mapBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath , tileName));
        }
        //AssetBundle mapBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath , tileName));
        return mapBundle;
    }

    public static TextAsset GetTiledById(string id)
    {
        if(mapAssetBundle == null) mapAssetBundle = GetTiledAssetBundle();
        TextAsset textAsset = mapAssetBundle.LoadAsset<TextAsset>($"Tile_{id}");
        return textAsset;
    }

    public static void UpdateLocalBundle(byte[] data, string filename)
    {
        byte[] results = data;
        
        #if UNITY_IOS
        string savePath = Path.Combine(Application.persistentDataPath , "iOS");
        #else
        string savePath = Path.Combine(Application.persistentDataPath , "Android");
        #endif
        
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
                    
        Debug.Log("save path:" + savePath + "/" + filename);
        FileInfo fileInfo = new FileInfo(savePath + "/" + filename);
        FileStream fs = fileInfo.Create();
        fs.Write(results, 0, results.Length);
        fs.Flush(); 
        fs.Close(); 
        fs.Dispose(); 
    }
    
    #if UNITY_EDITOR
    public static void BuildAssetBundle()
    {
        string platform = "iOS";
        #if UNITY_IOS
        platform = "iOS";
        #else
        platform = "Android";
        #endif
        string outputPath = "Assets/StreamingAssets/"+ platform;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        //根据BuildSetting里面所激活的平台进行打包
        BuildPipeline.BuildAssetBundles(outputPath, 0, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();

        Debug.Log("打包完成");

    }
    #endif
    
    
}
