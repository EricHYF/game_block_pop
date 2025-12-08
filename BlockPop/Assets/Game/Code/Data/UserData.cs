using System;
using System.Collections.Generic;
using System.IO;
using GKIT;
using JollyArt;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    
    /// <summary>
    /// 用户数据
    /// </summary>
    [Serializable]
    public class UserData
    {
        public string uid;
        public bool isGuide;
        public string userName; // 用户名称
        public int level;       // 游戏等级
        public int score;       // 游戏总得分
        public int goalScore;   // 目标分数
        public int gameState;    // 游戏状态
        public int luckyScore;  // 抽奖转盘得分
        public int userLevel;        // 用户评级
        public int userLevelExp; // 用户评级经验值
        // public List<UserTileData> tiles;
        

    }

    [Serializable]
    public class UserTileData
    {
        public int id;
        public int x;
        public int y;
    }

    /// <summary>
    /// 用户数据IO管理器
    /// </summary>
    public class UserDataIO
    {
        public const string SAVED_USER_DATA_NAME = "block_saved_user_data.bin";
        public const string DEFAULT_USER_NAME = "User01";

        public static string UserDataSaveDir
        {
            get
            {
                string dir = Application.persistentDataPath;
#if UNITY_EDITOR
                dir = Path.GetFullPath(Path.Combine(Application.dataPath, "../.simulated/SaveData"));
#endif
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                return dir;
            }
        }
        
        /// <summary>
        /// 用户存档路径
        /// </summary>
        public static string UserDataSaveFile => Path.Combine(UserDataSaveDir, SAVED_USER_DATA_NAME);
        /// <summary>
        /// 读取或者创建
        /// </summary>
        /// <returns></returns>
        public static UserData LoadOrCreat()
        {
            if (File.Exists(UserDataSaveFile))
            {
                string json = File.ReadAllText(UserDataSaveFile);
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        return ValidateData(JsonParser.ToObject<UserData>(json));
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
            return Create();
        }

        
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static UserData ValidateData(UserData data)
        {
            if (string.IsNullOrEmpty(data.userName)) data.userName = DEFAULT_USER_NAME;
            //TODO: 补全其他参数初始化验证
            return data;
        }
        
        
        
        
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        public static void Save(UserData data)
        {
            GLog.D("--- Save UserData  ---");
            string json = JsonParser.ToJson(data);
            File.WriteAllText(UserDataSaveFile, json);
        }

        public static UserData Create()
        {
            var data = new UserData()
            {
                uid = Guid.NewGuid().ToString(),
                userName = "User01",
                isGuide = true,
                level = 1,
                score = 0,
                goalScore = 0,
                luckyScore = 0,
                gameState = 0,
                userLevel = 0,
                // tiles = new List<UserTileData>(Const.GRID_COUNT),
                userLevelExp =  0,
            };
#if UNITY_EDITOR
            //TODO 编辑器参数注入
#endif
            return data;
        }


#if UNITY_EDITOR
        
        [MenuItem("卓利游戏/用户数据/删除用户数据", false, 0)]
        public static void EditorDeleteUserData()
        {
            DeleteUserData();
        }
        
        [MenuItem("卓利游戏/用户数据/打开存档路径", false, 0)]
        public static void EditorOpenUserDataPath()
        {
            OpenUserDataPath();
        }

        
        /// <summary>
        /// 删除用户数据
        /// </summary>
        public static void DeleteUserData()
        {
            if (File.Exists(UserDataSaveFile))
            {
                File.Delete(UserDataSaveFile);
            }
            PlayerPrefs.DeleteAll();
        }


        public static void OpenUserDataPath()
        {
            Application.OpenURL($"file://{UserDataSaveDir}");
        }
        
        
#endif
    }
    
    
}