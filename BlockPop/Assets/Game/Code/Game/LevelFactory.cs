using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    
    /// <summary>
    /// 关卡生成工厂
    /// </summary>
    public class LevelFactory
    {

        private static LevelClips _clips;
        public static LevelClips Clips
        {
            get
            {
                if(_clips == null) _clips = LevelClips.Load();
                return _clips;
            }
        }


        /// <summary>
        /// 创建一个关卡配置
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static LevelConfig BuildLevelConfig(int levelId,  int colors = 2)
        {
            var config = new LevelConfig()
            {
                id = levelId,
                colors = colors,
            };
            // for (int i = 0; i < Const.GRID_H; i++)
            // {
            //     stage[i] = "1,1,1,1,1,1,1,1,1,1";  //基本填充
            // }
            var list = new List<string>(Const.GRID_H);
            list.AddRange(MergeClips(Clips.GetRandomClip(), Clips.GetRandomClip()));
            list.AddRange(MergeClips(Clips.GetRandomClip(), Clips.GetRandomClip()));
            config.stage = list.ToArray();
            
            return config;
        }
        
        /// <summary>
        /// 横向合并两个Clip
        /// </summary>
        /// <param name="clipA"></param>
        /// <param name="clipB"></param>
        /// <returns></returns>
        private static string[] MergeClips(LevelClip clipA, LevelClip clipB)
        {
            string[] list = new string[clipA.Count];
            int i = 0;
            while (i < clipA.Count)
            {
                list[i] = $"{clipA.clip[i]},{clipB.clip[i]}";  // 横向拼接
                i++;
            }
            return list;
        }

    }
    
    

    
    
    
    
    
    
    
}