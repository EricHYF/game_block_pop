using System;
using JollyArt;

namespace Game
{
    // ---------- rotary_table_reward ----------
    // {
    //    "code":0 //0-成功 其他失败
    //    "msg": "绑定成功", //非0情况会给出对应msg 酌情处理
    //    "rewardType":1, //1大量红包 2中量红包 3少量红包 3少量红包 4提现机会 5下次翻倍
    //    "rewardValue":"1.33"//抽中的金额
    // }
    
    /// <summary>
    /// 转盘奖励数据
    /// </summary>
    [Serializable]
    public class RotaryRewardData : JsonDataBase
    {
        /// <summary>
        /// 奖励类型
        /// </summary>
        public int rewardType;
        
        /// <summary>
        /// 奖励金额
        /// </summary>
        public string rewardValue;
        
        public static RotaryRewardData Parse(string json)
        {
            return JsonParser.ToObject<RotaryRewardData>(json);
        }
    }
}