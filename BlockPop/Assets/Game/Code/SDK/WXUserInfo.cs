using System;
using JollyArt;



[Serializable]
public class WXUserInfo
{
    /// <summary>
    /// 用户的PhoneId
    /// </summary>
    public string userId;
    /// <summary>
    /// 微信用户名
    /// </summary>
    public string wx_uname;
    /// <summary>
    /// 微信头像的链接
    /// </summary>
    public string wx_uavatar;

    public static WXUserInfo Parse(string jsonData)
    {
        return JsonParser.ToObject<WXUserInfo>(jsonData);
    }
}
