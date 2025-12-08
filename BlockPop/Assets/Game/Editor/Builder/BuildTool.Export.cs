using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;


    
/// <summary>
/// 自动化构建管线
/// </summary>
public partial class BuildTool
{

    private const string BUILD_SEETING_PATH = "Game/Editor/Builder/BuildSetting.ini";
    private const string UNITY_LIBRARY_PATH = "unityLibrary";


    [MenuItem("Tools/自动化/一键出包", false, 51)]
    private static void EditorAutoPackage() => AutoExportBuildProject();


    public static void AutoExportBuildProject()
    {
        var fromPath = BuildAndroid(false, true) + $"/{UNITY_LIBRARY_PATH}";
        
        string[] lines = File.ReadAllLines(Path.Combine(Application.dataPath, BUILD_SEETING_PATH));
        if (lines.Length > 0)
        {
            string projPath = Path.GetFullPath($"{Application.dataPath}/{lines[0]}");
            string toPath = $"{projPath}/{UNITY_LIBRARY_PATH}";
            // 清理路径
            if (Directory.Exists(toPath))
            {
                DeletePath($"{toPath}/src");
                DeletePath($"{toPath}/libs");
                DeletePath($"{toPath}/build");
                DeletePath($"{toPath}/symbols");
            }

            // 拷贝文件
            if (Directory.Exists(fromPath))
            {
                Debug.Log($"--- 开始导入: {fromPath} \n\t--> {toPath}");

                CopyPath($"{fromPath}/src", $"{toPath}/src");
                CopyPath($"{fromPath}/libs", $"{toPath}/libs");
            }

#if UNITY_EDITOR_OSX
            Thread.Sleep(1000);
            var cmd = $"{projPath}/build.command";
            if (File.Exists(cmd))
            {
                Debug.Log("--- 调用打包脚本 ---");
                OpenPath(cmd);
            }
#endif
            
        }
        else
        {
            Debug.LogError("导出配置不存在");
        }
        

    }

    /// <summary>
    /// 删除路径
    /// </summary>
    /// <param name="path"></param>
    private static void DeletePath(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log($"--- Delete Path: {path}");
        }
    }

    private static bool CopyPath(string from, string to)
    {
        if (Directory.Exists(to)) DeletePath(to);
        if (!Directory.Exists(from))
        {
            Debug.LogError($"--- 拷贝路径不存在: {from}");
            return false;
        }

        FileUtil.CopyFileOrDirectory(from, to);
        return true;
    }
}
