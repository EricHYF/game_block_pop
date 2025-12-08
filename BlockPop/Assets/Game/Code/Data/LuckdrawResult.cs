using JollyArt;

namespace Game
{
    /// <summary>
    /// 抽奖结果
    /// </summary>
    public class LuckdrawResult : JsonDataBase
    {
        public string rewardValue;
        
        public static LuckydrawResult Parse(string json)
        {
            return JsonParser.ToObject<LuckydrawResult>(json);
        }
    }
}