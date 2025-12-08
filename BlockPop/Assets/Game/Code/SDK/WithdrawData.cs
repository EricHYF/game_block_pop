using System;
using System.Text;
using JollyArt;

[Serializable]
public class WithdrawItem
{
    public int id; //此次提现额度ID
    public string balance; //显示可提现金额
    public float redBeans;
    public int type; //type=1新人专享，type=2 无门槛提现
    public int residue; // 大于0或等于-1表示可提现，等于0表示次数用尽不可提现。
    public int daysUp; //（忽略）
    public string tipBar1;//注意事项
    public int totalDdays;//限制登录天数
    public int totalLevel;//限制等级
    public int daysLogin;

    public long expireTime;
    
    /// <summary>
    /// 是否可提现
    /// </summary>
    public bool CanWithdraw => residue != 0;

 
}

[Serializable]
public class WithdrawConfig
{
    public int daysLogin;//累计登陆
    public int dayCount;//当天领取合成红包次数
    public int level;//用户等级
    public int nextlevelTitleCount;
    public string[] tipBarList;//注意事项
    public bool serverLogin;
    public WithdrawItem[] withdrawConfigs;//提现选项 0.3. 300 500 1000
    public string GetTipBar()
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        for (int i = 0; i < tipBarList.Length; i++)
        {
            stringBuilder.Append(string.Format("{0}\r\n", tipBarList[i]));
        }

        return stringBuilder.ToString();
    }
}
    


[Serializable]
public class LuckyWithdrawConfig
{
    public long expireTimeStamp; //过期时间 毫秒时间戳
    public string rewardValue;  //抽中的提现金额

    public static LuckyWithdrawConfig[] ParseList(string json)
    {
        return JsonParser.ToObject<LuckyWithdrawConfig[]>(json);
    }
}



[Serializable]
public class WithdrawData : JsonDataBase
{
    public WithdrawConfig data;

    public static WithdrawData Parse(string json)
    {
        return JsonParser.ToObject<WithdrawData>(json);
    }
}

[Serializable]
public class WithdrawResult: JsonDataBase
{

    public bool refresh; //返回值为true时需要刷新提现列表页面

    public static WithdrawResult Parse(string json)
    {
        return JsonParser.ToObject<WithdrawResult>(json);
    }
}



[Serializable]
public class WithdrawRecodeItem
{
    public int id;
    public string amount;
    public string date;
    public int status;
    public string info;
    public string msg;
    
    
}




[Serializable]
public class WithdrawRecodeData : JsonDataBase
{
    public WithdrawRecodeItem[] data;

    public static WithdrawRecodeData Parse(string json)
    {
        return JsonParser.ToObject<WithdrawRecodeData>(json);
    }
}

