
using System;
using System.ComponentModel;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// SROptions 控制台菜单
/// </summary>
public partial class SROptions
{

    public const string K_CAT_DEBUG = "游戏调试";
    public const string K_CAT_PARAMS = "参数设置";
    public const string K_CAT_SIM_REDPACKET = "模拟红包";
    public const string K_CAT_LUCKYDRAW = "抽奖测试";
    public const string K_CAT_POPUPTEXT = "气泡提示";


    #region 调试接口

    [Category(K_CAT_DEBUG), DisplayName("TimeScale"), NumberRange(0, 10)]
    public float DebugTimeScale
    {
        get => Time.timeScale;
        set => Time.timeScale = value;
    }
    
    [Category(K_CAT_DEBUG), DisplayName("立即胜利")]
    public void DebugGameWin()
    {
        if (GameController.Instance == null) return;
         GameController.Instance.DebugSetGameWin();
    }
    
    
    

    #endregion
}