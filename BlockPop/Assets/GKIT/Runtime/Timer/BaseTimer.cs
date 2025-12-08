

using System;
using UnityEngine;

namespace GKIT
{

    public enum TimerType
    {
        Default = 0,
        Second = 1,
        Energy = 2,
        BuffEnergy = 3,
        BuffCoin = 4,
    }
    
    
    public class BaseTimer
    {
        public const string COUNTDOWN_FORMAT_MMSS = @"mm\:ss";
        public const string COUNTDOWN_FORMAT_HHMMSS = @"hh\:mm\:ss";
        public const string TIME_STAMP_FMT = "yyyy-MM-dd hh:mm:ss";


        private bool _isLoop;
        public bool IsLoop
        {
            get => _isLoop;
            set => _isLoop = value;
        } 
        public string ID => _data.id;
        private bool _isRunning;
        public bool IsRunning => _isRunning;
        public int duration;

        private TimerType _type;
        public TimerType Type
        {
            get=>_type;
            set => _type = value;
        }

        public bool IsTriggered {
            get
            {
                return DateTime.Now >= _startDate.AddSeconds(duration);
            }
        }

        public Action<BaseTimer> OnTrigger; 
        public Action<BaseTimer> OnComplete;
        public Action<BaseTimer> OnUpdate;

        public TimerData Data => _data;
        private TimerData _data;
        private int _loopTime;
        private float _passedSec;
        private DateTime _startDate;
        private DateTime _endDate;



        public BaseTimer()
        {
            _isRunning = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public BaseTimer(TimerData data)
        {
            _data = data;
            _isRunning = false;
            _startDate = _data.StartDate;
        }
        
        public BaseTimer(string  id)
        {
            _data = new TimerData(id);
            _isRunning = false;
            _startDate = _data.StartDate;
        }
        
        
        
        public virtual void Update()
        {
            if (!IsRunning) return;
            if (DateTime.Now >= _endDate)
            {
                if (!IsLoop)
                {
                    OnComplete?.Invoke(this);
                    Stop();
                }
                else
                {
                    RefreshStartDate();
                    OnTrigger?.Invoke(this);
                }
            }
            OnUpdate?.Invoke(this);
        }
        
        /// <summary>
        /// 刷新开始时间
        /// </summary>
        public void RefreshStartDate()
        {
            _startDate = DateTime.Now;
            _endDate = _startDate.AddSeconds(duration);
            _data.StartDate = _startDate;
        }

        public void Start()
        {
            if(_startDate.Year <= 1970) _startDate = DateTime.Now;
            _endDate = _startDate.AddSeconds(duration);
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }
        
        /// <summary>
        /// 转换为倒计时文字
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public string GetCountDownString(string fmt = BaseTimer.COUNTDOWN_FORMAT_HHMMSS)
        {
            var sp = _endDate - DateTime.Now;
            TimeSpan span = new TimeSpan(sp.Ticks);
            // span.Seconds
            return span.ToString(fmt);
        }


        public int GetTriggerTimes(DateTime endTime)
        {
            var span = endTime - _startDate;
            int times = 0;
            if (duration <= 0) duration = 180;
            times = (int)span.TotalSeconds / duration;
            return times;
        }

    }
}