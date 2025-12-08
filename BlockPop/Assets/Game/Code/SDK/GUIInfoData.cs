using System;
using Game;
    
[Serializable]
public class GUIInfoData
{
    public int phase;    // 0-不展示 1-第一阶段余额小于300元 2-第二阶段登录天数 3-第三阶段等级 4-防沉迷 5-首次进入
    public string firstValue;     // 第一个需要变色的字段 
    public string secondValue; // 第二个需要变色的字段
    public int showTime; //水果提示文案展示时间
    public int intervalTime; //水果提示文案间隔时间

    public static GUIInfoData Parse(string json)
    {
        return JsonParser.ToObject<GUIInfoData>(json);
    }

    public override string ToString()
    {
        return $"PHASE:{phase}===FIRSTVALUE:{firstValue}===SECONDVALUE:{secondValue}===SHOWTIME:{showTime}===INTERVALTIME:{intervalTime}";
    }
}

/// <summary>
/// 红点数据
/// </summary>
[Serializable]
public class RedotData
{
    public bool isShow;
    public static RedotData Parse(string json)
    {
        return JsonParser.ToObject<RedotData>(json);
    }
}
