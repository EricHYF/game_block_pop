using System;
using Game;
using JollyArt;


[Serializable]
public class LuckydrawData
{
    public int rewardId;    // 奖励ID
    public string info;     // 奖励信息
    public int rewardType;  // 1现金礼包 其他 提现机会 
    public string rewardValue; // 奖励金额
    public int expireTimeStamp;//1640227915051//过期时间 时间戳 毫秒

    public static LuckydrawData[] GetLuckydrawList(string json)
    {
        return JsonParser.ToObject<LuckydrawData[]>(json);
    }
}


[Serializable]
public class LuckydrawResult : JsonDataBase
{
    public int rewardId;
    public int rewardType; // 1现金礼包 其他 提现机会
    public string rewardValue; //对应奖励值
    public int rewardUpId; // 转盘ID
    
    public static LuckydrawResult Parse(string json)
    {
        return JsonParser.ToObject<LuckydrawResult>(json);
    }
}
    
