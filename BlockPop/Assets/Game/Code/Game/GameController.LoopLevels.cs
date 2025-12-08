using UnityEngine;

namespace Game
{
    
    public partial class GameController
    {


        #region 属性定义
        

        private static readonly string LevelJson1 = "Config/Loop/Sheet1";
        private static readonly string LevelJson2 = "Config/Loop/Sheet2";
        private static readonly string LevelJson3 = "Config/Loop/Sheet3";

        private const int GAME_STATE_START = 0;
        private const int GAME_STATE_CONTINUE = 1;
        private const int GAME_STATE_LOOP = 2;

        
        private LocalLevelConfig _headConfig;
        private LocalLevelConfig _loopConfig;

        private int _headLevelId;
        private int _loopLevelId;

        private int _curLoopLevelId;
        
        
        #endregion



        #region 新版启动游戏逻辑


        /// <summary>
        /// 初始化循环关卡配置
        /// </summary>
        private void InitLoopLevels()
        {
            _headLevelId = 1;
            _loopLevelId = 0;
            
            var config1= LocalLevelConfig.LoadFromFile(LevelJson1);
            var config2= LocalLevelConfig.LoadFromFile(LevelJson2);
            var config3= LocalLevelConfig.LoadFromFile(LevelJson3);

            if (User.GameLevel == 0 )
            {
                User.GameLevel = 1;
                User.GameState = GAME_STATE_START;
                _headConfig = config1;
                _loopConfig = config3;
                LoadFirstLevels();
            }
            else if (User.GameLevel <= config1.Count)
            {
                User.GameState = GAME_STATE_CONTINUE;
                _headConfig = config1;
                _loopConfig = config3;
                LoadFirstLevels();
            }
            else
            {
                User.GameState = GAME_STATE_LOOP;
                _headConfig = config2;
                _loopConfig = config3;
                LoadNormalLevels();
            }
        }

        /// <summary>
        /// 启动循环关卡
        /// </summary>
        private void StartLoopLevel()
        {
            UpdateLuckyScore();
            
            int[][] tiles = null;
            
            if (_curLevelConfig != null)
            {
                Debug.Log($"<color=#88ff00>--- 开始关卡: {_curLevelId} ---</color>");
                _score = 0;
                // _curLevelId = levelId;
                _goalScore = GetLevelGoalScore(_curLevelId);  //获取关卡分数
                tiles = _curLevelConfig.Tiles;
            }
            else
            {
                Debug.Log($"<color=red>--- 当前关卡配置不存在 ---</color>");
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



        private void GotoNextLevel()
        {
            _curLevelId++;
            _headLevelId++;
            if (User.GameState == GAME_STATE_START)
            {
                if (_headLevelId > _headConfig.Count)
                {
                    User.GameState = GAME_STATE_LOOP;
                    _loopLevelId = 1;
                    _curLevelConfig = _loopConfig.Get(_loopLevelId);
                    Debug.Log($"------- Set to Loop: <color=cyan>{_loopLevelId}/{_loopConfig.Count}</color>");
                }
                else
                {
                    _curLevelConfig = _headConfig.Get(_headLevelId);
                    User.GameState = GAME_STATE_CONTINUE;
                    Debug.Log($"------- Set to Continue: <color=cyan>{_headLevelId}/{_headConfig.Count}</color>");
                }
            }
            else
            {
                if (_headLevelId > _headConfig.Count)
                {
                    if (_loopLevelId < _loopConfig.Count)
                    {
                        _loopLevelId++;
                    }
                    else
                    {
                        _loopLevelId = 1;
                    }
                    // 开始循环Loop
                    _curLevelConfig = _loopConfig.Get(_loopLevelId);
                    Debug.Log($"------- Normal goto Loop: <color=cyan>{_loopLevelId}/{_loopConfig.Count}</color>");
                    User.GameState = GAME_STATE_LOOP;
                }
                else
                {
                    // 尚在头部循环中
                    _curLevelConfig = _headConfig.Get(_headLevelId);
                    Debug.Log($"------- Normal goto Head: <color=cyan>{_headLevelId}/{_headConfig.Count}</color>");
                }
            }
            
            StartLoopLevel();
        }
        

        #endregion

        #region 加载配置逻辑


        /// <summary>
        /// 加载首次启动的关卡
        /// </summary>
        private void LoadFirstLevels()
        {
            _headLevelId = User.GameLevel; // 加载未完成的关卡
            _loopLevelId = 0;
            Debug.Log("---- LoadFirstLevels ----");
            _curLevelId = User.GameLevel;
            if (_curLevelId == 0) _curLevelId = 1;
            _curLevelConfig = _headConfig.Get(_curLevelId);
            _curLoopLevelId = 0;
        }

        /// <summary>
        /// 加载普通启动的关卡
        /// </summary>
        private void LoadNormalLevels()
        {
            _headLevelId = 1;
            _loopLevelId = 0;
            Debug.Log("---- LoadNormalLevels ----");
            _curLevelId = User.GameLevel;
            _curLevelConfig = _headConfig.Get(1);
            _curLoopLevelId = 0;
        }



        #endregion
        
    }
}