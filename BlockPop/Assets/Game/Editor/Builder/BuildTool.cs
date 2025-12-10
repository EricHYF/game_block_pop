using System;
using System.Collections.Generic;
using System.IO;
using GKIT.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 打包工具
/// </summary>
public partial class BuildTool
{

    public const string DEFAULT_BUILD_NAME = "App";

    #region 菜单选项
    
    [MenuItem("Tools/构建包体/Android Project - Release", false, 31)]
    public static void BuildAndroidProject_Release()
    {
        BuildAndroid(false, true);
    }


    [MenuItem("Tools/构建包体/Android Project - Debug", false, 32)]
    public static void BuildAndroidProject_Debug()
    {
        BuildAndroid(true, true);
    }


    [MenuItem("Tools/构建包体/Android APK", false, 33)]
    public static void BuildAndroidAPK_Release()
    {
        BuildAndroid(false, false);
    }

    #endregion
    
    #region Common

    /// <summary>
    /// 获取构建场景
    /// </summary>
    /// <returns></returns>
    public static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (var e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }

        return names.ToArray();
    }

    /// <summary>
    /// 删除目录
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteDir(string path)
    {
        var dirInfo = new DirectoryInfo(path);

        if (dirInfo.Exists)
        {
            foreach (var file in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (file.Exists) file.Delete();
            }

            foreach (var dir in dirInfo.GetDirectories("*", SearchOption.AllDirectories))
            {
                DeleteDir(dir.FullName);
            }

            dirInfo.Delete();
            Debug.Log($"<color=orange>---- Delete Dir: {path} ----</color>");
        }

    }
    
    /// <summary>
    /// 生成构建号
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <returns></returns>
    public static string GenBuildNumber(BuildTarget buildTarget)
    {
        var nowDate = DateTime.Now;
        string strBuildNumber = $"{nowDate.Year - 2000}{nowDate.Month:00}{nowDate.Day:00}{((nowDate.Hour * 60 + nowDate.Minute) / 15):F00}";
        int buildNumber = int.Parse(strBuildNumber);
        if (buildTarget == BuildTarget.iOS)
        {
            PlayerSettings.iOS.buildNumber = strBuildNumber;
        }
        else if (buildTarget == BuildTarget.Android)
        {
            PlayerSettings.Android.bundleVersionCode = buildNumber;
        }
        return strBuildNumber;
        
    }

    /// <summary>
    /// 打开路径
    /// </summary>
    /// <param name="path"></param>
    private static void OpenPath(string path)
    {
        Application.OpenURL($"file://{path}");
    }

    #endregion
    
    #region Android 

    
    /// <summary>
    /// 构建Android项目
    /// </summary>
    /// <param name="isDebug"></param>
    /// <param name="isProject"></param>
    private static string BuildAndroid(bool isDebug, bool isProject = false)
    {
        string outPath = "";
        string outDir = Path.GetFullPath($"{Application.dataPath}/../Build");

        string buildNumber = GenBuildNumber(BuildTarget.Android);
        
        if (!isProject)
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            outPath = outDir + $"/app_{(isDebug ? "Debug" : "Release")}_{(Application.version)}_{buildNumber}.apk";
        }
        else
        {
            outPath = outDir + $"/{DEFAULT_BUILD_NAME}";
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        }
        
        // 清理输出路径
        if (Directory.Exists(outDir))
        {
            // DeleteDir(ourDir);
            Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);
        }
        
        // 构建新Bundle 
        GResBuilder.BuildBundles(true);
        
        // ------------------------- BUILD PROJECT -------------------------        
        Debug.Log($"---- 开始构建项目: {outPath} ----");
        var options = BuildOptions.None;
        if (isDebug)
        {
            options = BuildOptions.None | BuildOptions.Development | BuildOptions.AllowDebugging;
        }
        var buildPlayerOptions = new BuildPlayerOptions()
        {
            scenes = GetBuildScenes(),
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            locationPathName = outPath,
            options = options,
        };
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        OpenPath(outDir);

        return outPath;
    }
    
    #endregion
    
}
