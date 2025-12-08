using System;
using System.Collections;
using System.Collections.Generic;
using GKIT;
using GKIT.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    
    /// <summary>
    /// 游戏控制器
    /// </summary>
    public partial class GameController: MonoBehaviour
    {
        


        
        private static GameController _instance;
        public static GameController Instance => _instance;
        
        private WindowMainView _window;
        private LocalLevelConfig _localConfigs;
        private LevelConfig _curLevelConfig;
        // private Vendor Vendor => Vendor.Instance;
        private UIMan UI => UIMan.Instance;
        private UserMan User => UserMan.Instance;
        private SoundMan Sound => SoundMan.Instance;
        private float _lastSaveTime;

        


        /// <summary>
        /// 网格数据
        /// </summary>
        private Grid _grid;
        private int _score;
        private bool _levelHasStarted;  // 游戏提示保护， 未点击
        private int _curLevelId;
        private int _goalScore;

        public static GameController Create()
        {
            var window = UIMan.Instance.OpenWindow<WindowMainView>(G.Address.WindowMainView);
            if (window != null)
            {
                _instance = window.GameObject.AddComponent<GameController>();
                _instance._window = window;
                return _instance;
            }
            return null;
        }

        #region 生命周期
        
        private void Awake()
        {
            _instance = this;
            // _localConfigs = LocalLevelConfig.Load();
            _lastSaveTime = 0;
            AddEvent(Events.OnClickTile, OnClickTile );
            AddEvent(Events.OnShowMergeInfo, OnShowMergeInfo );

            InitLoopLevels();  // 初始化循环关卡
        }

        private void AddEvent(Events evt, Action<object> callback)
        {
            GEvent.Bind(evt.ToString(), callback );
        }


        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus) SaveGame();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus) SaveGame();
        }

        private void OnApplicationQuit()
        {
            SaveGame(true);
        }

        #endregion

        #region 游戏状态

        private bool _isPlayingState;
        public bool IsPlayingState => _isPlayingState;


        /// <summary>
        /// 开始游戏
        /// </summary>
        public void StartGame()
        {
            _isPlayingState = false;
            _score = 0;
            _levelHasStarted = false;
            InitGrid();
#if UNITY_IOS
            _window.HideBulletScreen();
#else
            InitBulletScreen();
#endif
            
            InitGuide(() =>
            {
                // if (User.UserTiles != null && User.UserTiles.Count > 0)
                // {
                //     // 有用户数据
                //     StartLevel();
                // }
                // else
                // {
                //     StartLevel(User.GameLevel);
                // }
                
                StartLoopLevel();
            });
        }



        

        #endregion

        #region 关卡控制

        public void StartLevel(int levelId = 0)
        {
            UpdateLuckyScore();
            
            int[][] tiles = null;
            
            if (levelId == 0)
            {
                // 默认值则加载用户存档
                tiles = LoadGame();
            }
            else
            {
                // 进入新关卡
                _score = 0;
                _curLevelId = levelId;
                _goalScore = GetLevelGoalScore(_curLevelId);  //获取关卡分数
                _curLevelConfig = GetLevelConfig(_curLevelId);
                tiles = _curLevelConfig.Tiles;
            }
            
            if (null == _curLevelConfig)
            {
                Debug.Log($"<color=red>--- 关卡不存在：{_curLevelId} ---</color>");
                return;
            }
            if (null == tiles)
            {
                Debug.Log($"<color=red>--- 区块配置不存在 ! ---</color>");
                return;
            }
            
            _window.SetScore(_score);
            _window.EnterLevel(_curLevelId, _goalScore, () =>
            {
                var icons = _window.TileSprites.GetSprites(_curLevelConfig.colors); // 直接获取图标
                _grid.InitTiles(tiles, icons);
                _isPlayingState = true;
                SaveGame(true);
            });


        }
        
        // 获取关卡目标
        private int GetLevelGoalScore(int level)
        {
            return Random.Range(4000, 8000);  
        }

        // 关卡结束
        private void OnFinishLevel()
        {
            StartCoroutine(nameof(OnFinishing));
        }

        private IEnumerator OnFinishing()
        {
            _isPlayingState = false;
            CancelHint();
            // Vendor.GetRedPacketConfig(RedPacketType.Combine);
            UpdateLuckyScore();
            
            var time = 2.5f;
            Sound.Play(Sound.SFX.FXFireworks);
            yield return new WaitForSeconds(time);

            CheckWithdrawClaimPopup();
            // RedPacket.OnComboReceive(_redpacketData, OnClose);

            int next = _curLevelId +1;
            // StartLevel(next);
            
            // 完成关卡
            Debug.Log($"<color=#88ff00>------ OnFinishLevel -> Next Level：{next}</color>");

            GotoNextLevel();
        }
   
        
        /// <summary>
        /// 点击网格
        /// </summary>
        /// <param name="data"></param>
        private void OnClickTile(object data)
        {
            CancelHint();
            if (!_levelHasStarted)  _levelHasStarted = true;
            if(_levelHasStarted) DelayShowHint();
            
        }

        /// <summary>
        /// 检查是否可以弹出提现弹窗
        /// </summary>
        private void CheckWithdrawClaimPopup()
        {
            // #1 开关打开
            // #2 关卡达到提现关
            // #3 有提现可领取
            // #4 从未弹出过该面板
            
            // if (Vendor.IsOpenWithdrawTip 
            //     && _curLevelId == Vendor.Withdraw3Limit
            //     && Vendor.HasWithdraw3
            //     && !Vendor.HasShownWithdrawClaimPop)
            // {
            //     Vendor.HasShownWithdrawClaimPop = true;
            //     // 开关开启, 且满足0.3提现关卡要求, 弹出可领取弹窗
            //     UI.OpenPopup<PopupFirstWithdrawClaim>(G.Address.PopupFirstWithdrawClaim);
            // }
        }
        

        #endregion
        
        #region 初始化盘面
        /// <summary>
        /// 初始化网格
        /// </summary>
        private void InitGrid()
        {
            if (_grid == null)
            {
                Debug.Log($"------ Init Grid -------");
                _grid = new Grid();
                _grid.Init(_window.TileRoot, _window.TilePrefab, _window.ScorePrefab, _window.ScoreNode );
                _grid.OnAddScore = OnAddScore;
                _grid.OnFinishLevel = OnFinishLevel;
            }
        }


        private void OnShowMergeInfo(object data)
        {
            _window.ShowMergeInfo();
        }
        
        
        #endregion
        
        #region 分数上报

        private void OnAddScore(int score)
        {
            _score += score;
            _window.SetScore(_score);
        }
        

        #endregion

        #region 提示设置

        private void DelayShowHint()
        {
            CancelHint();
            StartCoroutine(nameof(OnWaitingHint));
        }

        private void CancelHint()
        {
            StopCoroutine(nameof(OnWaitingHint));
            _grid.CancelHint();
        }
    
        /// <summary>
        /// 显示提示
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnWaitingHint()
        {
            yield return new WaitForSeconds(Const.HINT_TIME);
            _grid.ShowHint();
        }
       
        

        #endregion

        #region 特效控制

        public void ShowRedPacketFlying() => _window.ShowRedPacketFlying();

        #endregion
        
        #region 抽奖管理

        /// <summary>
        /// 更新分数
        /// </summary>
        private void UpdateLuckyScore()
        {
            // if (!Vendor.IsBusinessOpen) return;
            // _window.UpdateLocalLuckyDraw();
        }
        
        #endregion

        #region 数据处理

        /// <summary>
        /// 获取LevelConfig
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        private LevelConfig GetLevelConfig(int levelId, int colors = 2)
        {
            var config = _localConfigs.Get(levelId);
            if (config == null)
            {
                // 超出固定关卡范围
                config = LevelFactory.BuildLevelConfig(levelId, colors);
            }
            return config;
        }

        /// <summary>
        /// 保存盘面
        /// </summary>
        public void SaveGame(bool forceSave = false)
        {
            if (forceSave || CanSaveNow())
            {
                User.GameLevel = _curLevelId;
                // User.UserTiles = _grid.ToUserTiles();
                User.Score = _score;
                User.GoalScore = _goalScore;
                User.Save();
            }
        }
        
        /// <summary>
        /// 存储抬手期
        /// </summary>
        /// <returns></returns>
        private bool CanSaveNow()
        {
            float delta = Time.timeSinceLevelLoad - _lastSaveTime;
            if (delta > Const.SAVE_INTERVAL)
            {
                _lastSaveTime = Time.timeSinceLevelLoad;
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public int[][] LoadGame()
        {
            Debug.Log($"<color=orange>--- Load Game ---</color>");
            _curLevelId = User.GameLevel;
            _score = User.Score;
            _goalScore = User.GoalScore;
            _curLevelConfig = GetLevelConfig(_curLevelId);
            // if(FromUserTiles(User.UserTiles, out var tiles))
            // {
            //     return tiles;
            // }
            return _curLevelConfig?.Tiles;
        }

        /// <summary>
        /// 字符串数据转布局
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool FromUserTiles(List<UserTileData> list, out int[][] tiles)
        {
            int tid = 0;
            tiles = new int[Const.GRID_H][];
            for (int i = 0; i < list.Count; i++)
            {
                var data = list[i];
                
                if (tiles[data.y] == null) tiles[data.y] = new int[Const.GRID_W];
                tiles[data.y][data.x] = data.id;
                tid += data.id;
            }
            return tid > 0;
        }
        
        
        
        
        

        #endregion
        
        #region 弹幕接口

        private void InitBulletScreen()
        {
            _window.InitBulletScreen();
        }
        
        

        /// <summary>
        /// 弹幕控制开关
        /// </summary>
        public bool BulletScreenSwitch
        {
            get
            {
                if (PlayerPrefs.HasKey("blocks_bullet_switch"))
                {
                    return PlayerPrefs.GetInt("blocks_bullet_switch",0) == 1;
                }
                else
                {
                    BulletScreenSwitch = true;
                    return true;
                }
            }
            set
            {
                PlayerPrefs.SetInt("blocks_bullet_switch", (value ? 1 : 0));
                PlayerPrefs.Save();
            }
        }

        #endregion

        #region 新人红包
        
        /// <summary>
        /// 是否展示过新人红包
        /// </summary>
        private bool HasShownGuideMoney
        {
            get => PlayerPrefs.GetInt("has_show_guide_money") == 1;
            set
            {
                PlayerPrefs.SetInt("has_show_guide_money", value? 1: 0);
                PlayerPrefs.Save();
            }
        }
        
        /// <summary>
        /// 初始化引导
        /// </summary>
        /// <param name="callback"></param>
        private void InitGuide(Action callback)
        {
// #if UNITY_IOS
            // iOS 模式下不显示引导红包
            callback?.Invoke();
            // return;
// #endif

            // if (Vendor.IsFirstGuideClose 
            //     || HasShownGuideMoney)
            // {
            //     // 引导关闭或者已显示, 则跳过
            //     callback?.Invoke();
            //     return;
            // }
            // var popup = UI.OpenPopup<PopupGuideMoney>(G.Address.PopupGuideMoney);
            // popup.OnPopupClose = () =>
            // {
            //     HasShownGuideMoney = true;
            //     callback?.Invoke();
            // };
            
        }
        

        #endregion

        #region DEBUG
        
        /// <summary>
        /// 作弊完成游戏
        /// </summary>
        public void DebugSetGameWin()
        {
            OnFinishLevel();
        }
        

        #endregion
    }
        
        
    
}