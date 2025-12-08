using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 图标配置
/// </summary>
[CreateAssetMenu(fileName = "LogoSwitchConfig", menuName = "Game/Create LogoSwitchConfig",order = 10)]
public class LogoSwitchConfig : ScriptableObject
{
    [Header("组件Prefab路径")]
    public string assetPath;
    [Header("组件目标物路径")]
    public string targetPath;
    [Header("组件图标配置")]
    public LogoSetting[] settings;
    
    public static readonly string LogoSwitchConfigPath = "Assets/Game/Editor/LogoSwitcher/LogoSwitchConfig.asset";

#if UNITY_EDITOR
    public static LogoSwitchConfig EditorLoading()
    {
        if (File.Exists(LogoSwitchConfigPath))
        {
            var settings = AssetDatabase.LoadAssetAtPath<LogoSwitchConfig>(LogoSwitchConfigPath);
            return settings;
        }
        Debug.LogError($">> 文件不存在: {LogoSwitchConfigPath}");
        return null;
    }
#endif


}




[Serializable]
public class LogoSetting
{
    public string name;
    public Sprite loadingLogo;
    public Sprite splash;
}
