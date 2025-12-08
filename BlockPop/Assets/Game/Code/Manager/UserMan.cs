using System;
using System.Collections.Generic;
using System.Linq;
using GKIT;
using GKIT.Event;
using UnityEngine;

namespace Game
{
    
    /// <summary>
    /// 用户数据管理器
    /// </summary>
    public class UserMan: GSingleton<UserMan>
    {
        private UserData _userData;
        public UserData UserData => _userData; 
        private List<TimerData> timers;
        private List<string> tutorials;
        private bool _isDataReady = false;
        
        public int UserLevel => UserData.userLevel;

        public int GameLevel
        {
            get => UserData.level;
            set => UserData.level = value;
        }

        // public List<UserTileData> UserTiles
        // {
        //     get => UserData.tiles;
        //     set => UserData.tiles = value;
        // }
        
        public int Score
        {
            get => UserData.score;
            set => UserData.score = value;
        }
        
        public int GoalScore
        {
            get => UserData.goalScore;
            set => UserData.goalScore = value;
        }

        public int GameState
        {
            get => UserData.gameState;
            set => UserData.gameState = value;
        }

        public Texture2D UserIcon { get; set; }


        #region 初始化
        
        public void Init()
        {
            // 加载用户数据
            _userData = UserDataIO.LoadOrCreat();
            InitUserData();
        }

        private void InitUserData()
        {
            _isDataReady = true;
            timers = new List<TimerData>();
        }

        private void InitBusi()
        {
            //绑定用户余额
            // _vendor.GetUserMoney(GetUserMoney);
            //是否开启商业化
            // _isOpenBusiness = _vendor.IsBusinessOpen;
            //登陆日期
            this._loginTime = PlayerPrefs.GetInt("LOGINTIME",0);
            
            //跨天添加道具
            // if (this._loginTime < _vendor.ServerTimeDate.DayOfYear)
            // {
            //     // AddHammmerNum(1);
            //     // AddBombNum(1);
            // }
            //记录登陆日期
            // LoginTime = _vendor.ServerTimeDate.DayOfYear;
        }

        private void InitDefault()
        {
            //登陆日期
            this._loginTime = PlayerPrefs.GetInt("LOGINTIME",0);
            //跨天添加道具
            if (this._loginTime <  DateTime.Now.DayOfYear)
            {
                // AddHammmerNum(1);
                // AddBombNum(1);
            }

            LoginTime = DateTime.Now.DayOfYear;
        }


        #endregion

        #region 工具接口
        
        /// <summary>
        /// 保存数据
        /// </summary>
        public void Save()
        {
            UserDataIO.Save(_userData);
        }




        #endregion        

        #region 计时器接口

        public List<TimerData> Timers => timers;

        
        
        public void AddTimerData(TimerData data)
        {
            foreach (var t in timers)
            {
                if (t.id == data.id)
                {
                    t.StartDate = data.StartDate;
                    return;
                }
            }
            timers.Add(data);
        }

        public void RemoveTimerData(TimerData data)
        {
            if (timers.Contains(data))
            {
                timers.Remove(data);
            }
        }

        public TimerData GetTimerData(string id)
        {
            return timers.FirstOrDefault(c => c.id == id);
        }
        

        #endregion
        
        #region 教程接口
        
        /// <summary>
        /// 步骤是否完成
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsTutoStepFinished(string key)
        {
            return tutorials.Contains(key);
        }
        
        /// <summary>
        /// 设置某个步骤完成
        /// </summary>
        /// <param name="key"></param>
        public void SetTutorialFinished(string key)
        {
            Debug.Log($"---- Tutorial Finish: {key}");
            tutorials.Add(key);   
        }
        
        /// <summary>
        /// 清除所有的步骤
        /// </summary>
        public void ClearAllSteps()
        {
            tutorials.Clear();
        }
        

        #endregion

        #region 玩家余额接口

        private float _userMoney = 0;
        private float _lastMoney = 0;

        public float UserMoney
        {
            get => this._userMoney;
            set => this._userMoney = value;
        }
        
        public float LastMoney
        {
            get => this._lastMoney;
            set => this._lastMoney = value;
        }

        private void GetUserMoney(float money)
        {
            _lastMoney = _userMoney;
            _userMoney = money;
            // GEvent.Send(G.InGame.USERMONEYEVENT);
        }
        
        #endregion
        
        #region 记录玩家登陆日期

        private int _loginTime = 0;

        public int LoginTime
        {
            set
            {
                PlayerPrefs.SetInt("LOGINTIME",value);
                this._loginTime = value;
            }
        }


        #endregion
        
        #region 引导接口

        private const string K_GUIDE_HAND_FINISHED = "guide_hand_finished";
        private const string K_GUIDE_BALL_FINISHED = "guide_ball_finished";

        public bool IsGuideHandFinished
        {
            get => PlayerPrefs.GetInt(K_GUIDE_HAND_FINISHED,0) == 1;
            set
            {
                PlayerPrefs.SetInt(K_GUIDE_HAND_FINISHED, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public bool IsGuideBallFinished
        {
            get => PlayerPrefs.GetInt(K_GUIDE_BALL_FINISHED, 0) == 1;
            set
            {
                PlayerPrefs.SetInt(K_GUIDE_BALL_FINISHED, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        #endregion
        
    }
}