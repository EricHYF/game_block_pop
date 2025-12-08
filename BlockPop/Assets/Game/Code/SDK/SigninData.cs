using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace JollyArt.Data
{
    
    /// <summary>
    /// 抽奖配置
    /// </summary>
    [Serializable]
    public class SigninData : JsonDataBase
    {
        public int allDays;
        public SigninRewardConfig[] rewardConfList;
        public static SigninData Parse(string json)
        {
            return JsonParser.ToObject<SigninData>(json);
        } 
    }

    /// <summary>
    /// 抽奖奖励配置
    /// </summary>
    [Serializable]
    public class SigninRewardConfig
    {
        public int rewardType;
        public int targetTag;
    }

}