using System.Collections;
using GKIT;
using UnityEngine;

namespace Game
{
    public class GameEntry: GSingletonMono<GameEntry>
    {


        #region 各管理器

        public static UIMan UI;
        public static ResMan Res;
        public static VibrationMan Vibration;
        public static SoundMan Sound;
        public static UserMan User;
        public static TimerMan Timer;
        // public static Vendor Vendor;
        // public static RedPacketMan Redpacket;
        public static GameController GameCon;
        
        #endregion




        /// <summary>
        /// 是否开启调试模式
        /// </summary>
        public static bool IsDebug
        {
            get
            {
#if UNITY_EDITOR
                return true;
#endif
                return Debug.isDebugBuild;   
            }
        }


        void Awake()
        {
            Init();
        }
        
        void Start()
        {
            StartUp();
        }
        


        #region 游戏启动

        private void Init()
        {
            // if (IsDebug)
            // {
#if UNITY_EDITOR
            SRDebug.Init();
#endif
            // }
            Debug.Log("----------- Unity StartUp ------------------");
            UI = UIMan.Instance;
            UI.ShowLoadNode();
        }
        
        public void StartUp()
        {
            ResMan.StartPreLoad(()=>
            {
                UI.SetLoadingProgress(1);
                StartCoroutine(nameof(InitManagers));
            }, 
            p =>
            {
                UI.SetLoadingProgress(p);
            });
        }
        
        /// <summary>
        /// 初始化所有顶级管理器
        /// </summary>
        private IEnumerator InitManagers()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Vibration = VibrationMan.Instance;
            User = UserMan.Instance;
            Sound = SoundMan.Instance;
            Timer = TimerMan.Instance;
            // Vendor = Vendor.Instance;
            // Redpacket = RedPacketMan.Instance;

            // SDK Init
            
            User.Init();
            Sound.Init();
            Timer.Init();
            // Redpacket.Init();

            yield return null;
            
            Vibration.VibrationEnabled = true;
            
            // CheckFirstOpen(); // 进行首次启动检查
            
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            yield return new WaitForSeconds(0.2f);
            
            EnterGameLevel();
        }
               
        /// <summary>
        /// 加载游戏主场景
        /// </summary>
        private void EnterGameLevel()
        {
            UI.HideLoadNode();
            GameCon = GameController.Create();
            GameCon.StartGame();
        }

        #endregion
        
        
        
    }
}