
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GKIT
{
    /// <summary>
    /// 配置化SO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigrationSO<T> : ScriptableObject where T : ScriptableObject
    {
        protected const string CONFIG_PATH = "Config/";
        private static readonly object lockobj = new object();
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        string path = $"{CONFIG_PATH}{typeof(T).Name}";
                        _instance = Resources.Load<T>(path);
                        if (_instance == null)
                        {
                            _instance = CreateInstance<T>();
                        }
                    }
                }
                return _instance;
            }
        }

        protected virtual void InitDefault()
        {
            
        }

        public static bool IsExistInstance() => _instance != null;
    }
    
}
    