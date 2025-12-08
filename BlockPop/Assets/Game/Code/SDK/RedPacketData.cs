using System;
using Game;
using JollyArt;

[Serializable]
public class RedPacketConfig
{
    public string configId; //此次请求获取的配置的ID值，每次获取都不一样
    public double value;    // 红包金额
    public bool antiMode;   // 是否进入防沉迷模式 true进入
    public int level;   // 用户等级
    public bool huanChongStatus; // 缓冲区间 
    public int source;//红包类型 对应领取时的配置sourceId
    public int realSource;
    public bool showRedUI; // True的话显示红包, 为False的话直接领取红包
    public bool hasExtraRewardv2;    // 是否是强弹广告红包
    
    public override string ToString()
    {
        return $"CONFIGID:{configId}===VALUE:{value}===ANTIMODE:{antiMode}==LEVEL:{level}===HUANCHONGSTATUS:{huanChongStatus}==SOURCE:{source}";
    }
}

[Serializable]
public class RedPacketData : JsonDataBase
{
    public RedPacketConfig config;
    
    public static RedPacketData Parse(string json)
    {
        return JsonParser.ToObject<RedPacketData>(json);
    }
}

[Serializable]
public class RedPacketRewardData: JsonDataBase
{
    public double amount;
    public int realSource;//红包类型 对应领取时的配置sourceId
    public bool huanChongStatus;    // 是否进入了缓冲状态
    
    public static RedPacketRewardData Parse(string json)
    {
        return JsonParser.ToObject<RedPacketRewardData>(json);
    }
}
    
    
