using System;
using System.Linq;
using JollyArt;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class LevelConfig
    {
        public int id;
        public int colors;
        public string[] stage;
        public int Count => stage.Length;
        
        public int[][] Tiles
        {
            get
            {
                int[][] tiles = new int[stage.Length][];
                for (int i = 0; i < stage.Length; i++)
                {
                    tiles[i] = Array.ConvertAll(stage[i].Split(','), int.Parse);
                }
                return tiles;
            }
        }

    }


    [Serializable]
    public class LocalLevelConfig
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        public int id;
        /// <summary>
        /// 关卡配置列表
        /// </summary>
        public LevelConfig[] configs;

        public int Count => configs.Length;


        /// <summary>
        /// 加载关卡配置
        /// </summary>
        /// <returns></returns>
        public static LocalLevelConfig Load() => LoadFromFile("Config/level_config");
        
        /// <summary>
        /// 加载指定关卡配置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static LocalLevelConfig LoadFromFile(string path)
        {
            TextAsset ta = Resources.Load<TextAsset>(path);
            if (ta != null)
            {
                string json = ta.text;
                return JsonParser.ToObject<LocalLevelConfig>(json);
            }
            return null;
        }


        public LevelConfig Get(int id)
        {
            return configs.FirstOrDefault(c => c.id == id);
        }
        
        
    }
    
    
}