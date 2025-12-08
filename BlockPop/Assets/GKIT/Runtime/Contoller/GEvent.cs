using System;
using System.Collections.Generic;
using UnityEngine;

namespace GKIT.Event
{
    public class GEvent
    {
        private static Dictionary<string, Action<object>> _eventTable;
        public static Dictionary<string, Action<object>> EventTable
        {
            get
            {
                if (_eventTable == null) _eventTable = new Dictionary<string, Action<object>>(50);
                return _eventTable;
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public static void Bind(string eventName, Action<object> callback)
        {
         
            if (EventTable.ContainsKey(eventName))
            {
                EventTable[eventName] += callback;
            }
            else
            {
                EventTable[eventName] = callback;
            }
        }
        
        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public static void Release(string eventName, Action<object> callback)
        {
            if (Has(eventName))
            {
                EventTable[eventName] -= callback;
                if (EventTable[eventName]==null || EventTable[eventName].GetInvocationList().Length == 0)
                {
                    EventTable.Remove(eventName);
                }
            }
        }

        public static bool Has(string eventName)
        {
            return EventTable.ContainsKey(eventName);
        }

        public static void Send(string eventName, object data = null)
        {
            if (Has(eventName))
            {
                EventTable[eventName]?.Invoke(data); // 发送消息
            }
        }
        
    }
}