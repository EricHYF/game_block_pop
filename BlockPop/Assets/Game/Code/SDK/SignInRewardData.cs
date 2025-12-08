namespace JollyArt.Data
{

    /// ------------ sign_in_reward -------
    /// {
    ///     "code":0 //0-成功 其他失败
    ///     "msg": "绑定成功", //非0情况会给出对应msg 酌情处理
    ///     "amount":"1.33"//签到得到的金额
    /// }
    
    public class SignInRewardData: JsonDataBase
    {
        // 得到奖励的金额
        public string amount;
        
        public static SignInRewardData Parse(string json)
        {
            return JsonParser.ToObject<SignInRewardData>(json);
        }
    }
}