using System;
using System.Collections.Generic;

namespace GKIT
{
    
    [Serializable]
    public class TimerData
    {
        /// <summary>
        /// 计时器ID
        /// </summary>
        public string id = "";

        private DateTime _startDate = new DateTime(1970, 1,1);
        public DateTime StartDate
        {
            get
            {
                if (_startDate.Year == 1970)
                {
                    _startDate = DateTime.Now;
                }
                return _startDate;
            }

            set => _startDate = value;
        }

        public TimerData() { }

        public TimerData(string id)
        {
            this.id = id;
        }
    }
}