using System;
using System.Collections.Generic;
using System.Linq;
using GKIT;
using UnityEngine;

namespace Game
{
    
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimerMan : GSingleton<TimerMan>
    {
        public const string TIMER_ID_ENERGY = "timer_energy"; // 体力计时器
        public const string TIMER_ID_BUFFENERGY = "timer_buff_energy"; //
        public const string TIMER_ID_BUFFCOIN = "timer_buff_coin";
        public const string TIMER_ID_TIMINGMONEY = "timer_timing_Money";

        private List<BaseTimer> _timerList;
        private UserMan User;
        private BaseTimer _energyTimer;
        private BaseTimer _buffEnergyTimer;
        private BaseTimer _buffCoinTimer;

        public void Init()
        {
            User = UserMan.Instance;
            // 注册秒事件
            _timerList = new List<BaseTimer>(Mathf.Max(UserMan.Instance.Timers.Count, 10));
        }
        
        /// <summary>
        /// 设置并添加计时器
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public BaseTimer SetupTimer(TimerData data)
        {
            var timer = new BaseTimer(data);
            _timerList.Add(timer);
            User.AddTimerData(data);
            return timer;
        }


        /// <summary>
        /// 秒事件
        /// </summary>
        /// <param name="delta"></param>
        private void OnSecond(float delta)
        {
            foreach (var timer in _timerList)
            {
                if (timer != null && timer.IsRunning)
                {
                    timer.Update();
                }
            }
        }



        #region 计时器管理
        

        /// <summary>
        /// 获取计时器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseTimer GetTimer(string id)
        {
            return _timerList.FirstOrDefault(c => c.ID == id);
        }
        

        /// <summary>
        /// 创建计时器
        /// </summary>
        /// <param name="id"></param>
        /// <param name="duration"></param>
        /// <param name="triggerHandle"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BaseTimer CreateTriggerTimer(string id, int duration, Action<BaseTimer> triggerHandle, Action<BaseTimer> updateHandle, TimerType type = TimerType.Default)
        {
            TimerData data = UserMan.Instance.GetTimerData(id);
            if (data == null) data = new TimerData(id);
            var t = Instance.SetupTimer(data);
            t.duration = duration;
            t.Type = type;
            t.OnTrigger = triggerHandle;
            t.OnUpdate = updateHandle;
            return t;
        }

        public BaseTimer GetTimer(string id, int duration, Action<BaseTimer> triggerHandle, TimerType type = TimerType.Default)
        {
            TimerData data = UserMan.Instance.GetTimerData(id);
            if (data != null)
            {
                var t = Instance.SetupTimer(data);
                t.duration = duration;
                t.Type = type;
                t.OnTrigger = triggerHandle;
                return t;
            }
            return null;
        }
        
        public static BaseTimer CreateTimer(string id, int duration, Action<BaseTimer> updateHandle, Action<BaseTimer> complete, TimerType type = TimerType.Default)
        {
            TimerData data = UserMan.Instance.GetTimerData(id);
            if (data == null) data = new TimerData(id);
            var t = Instance.SetupTimer(data);
            t.duration = duration;
            t.Type = type;
            t.OnUpdate = updateHandle;
            t.OnComplete = complete;
            t.Start();
            return t;
        }

        public BaseTimer GetTimer(string id, int duration, Action<BaseTimer> updateHandle, Action<BaseTimer> complete, TimerType type = TimerType.Default)
        {
            TimerData data = UserMan.Instance.GetTimerData(id);
            if (data != null)
            {
                var t = Instance.SetupTimer(data);
                t.duration = duration;
                t.Type = type;
                t.OnUpdate = updateHandle;
                t.OnComplete = complete;
                return t;
            }
            return null;
        }
        

        #endregion




        
    }
}