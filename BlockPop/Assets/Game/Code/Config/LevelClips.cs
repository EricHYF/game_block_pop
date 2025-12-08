using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    [Serializable]
    public class LevelClip
    {
        public int id;
        public string[] clip;
        public int Count => clip.Length;
    }


    [Serializable]
    public class LevelClips
    {
        public LevelClip[] color2;

        private int _randIndex2 = -1;
        private int[] _randID2;

        public LevelClip GetClipColor2(int id) => color2.FirstOrDefault(c => c.id == id);
        
        
        
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public static LevelClips Load()
        {
            var ta = Resources.Load<TextAsset>("Config/level_clips");
            return ta != null ? JsonParser.ToObject<LevelClips>(ta.text) : null;
        }

        /// <summary>
        /// 随机获得Clip对象
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public LevelClip GetRandomClip(int colors = 2)
        {
            if (colors == 2)
            {
                if (_randIndex2 == -1 || _randIndex2 >= _randID2.Length)
                {
                    ShuffleColor2();
                }


                var clip = GetClipColor2(_randID2[_randIndex2]);
                _randIndex2++;
                return clip;
            }
            return null;
        }
        
        /// <summary>
        /// 颜色2 洗牌
        /// </summary>
        public void ShuffleColor2()
        {
            _randID2 = new int[color2.Length];
            int i = 0;
            while (i < color2.Length)
            {
                _randID2[i] = color2[i].id;
                i++;
            }
            i = 0;
            while (i < color2.Length)
            {
                var rid = Random.Range(i, color2.Length);
                if (rid < color2.Length - 2)
                {
                    (color2[i], color2[rid]) = (color2[rid], color2[i]);
                }
                i++;
            }
            _randIndex2 = 0;
        }



        
        
        
    }

}