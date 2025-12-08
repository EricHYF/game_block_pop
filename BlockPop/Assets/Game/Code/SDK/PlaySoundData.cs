using System;
using JollyArt;

namespace Game
{
    [Serializable]
    public class PlaySoundData: JsonDataBase
    {
        /// <summary>
        ///  是否开启音效
        /// </summary>
        public bool enabled;
        
        public static PlaySoundData Parse(string json)
        {
            return JsonParser.ToObject<PlaySoundData>(json);
        } 
    }
}