using System;
using Game;
using JollyArt;



public enum PopTaskStatus
{
    Normal = 0,
    Claimed = 1,
    CanClaim = 2,
}

/// <summary>
/// 弹窗式任务数据
/// </summary>
[Serializable]
public class PopTaskData
{
    public int taskId;
    public string amount;
    public int status;  //1已经领取 2可领取 其他未领取
    public int extNum;  //合成指定球球的数值 如 512、1024等
    public int tagNum;  //完成任务需要的对应任务类型的数量 比如需要合成2个2048
    public int rewardType;  //1：提现 2，现金奖励
    public int nextExtNum;  //下一个任务需要的提示
    
    
    
    public static PopTaskData Parse(string json)
    {
        return JsonParser.ToObject<PopTaskData>(json);
    }


    #region 数据接口

    public bool IsWithdrawTask => rewardType == 1;

    public PopTaskStatus Status 
    {
        get{
            if (status == 1) return PopTaskStatus.Claimed;
            if (status == 2) return PopTaskStatus.CanClaim;
            return PopTaskStatus.Normal;
        }
    }

    #endregion

}
