using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GKIT
{
    /// <summary>
    /// 单利基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GSingleton<T> where T : class
    {
        private static object _obj = new object();
        private static T _instance;
        public static T Instance
        {
            get
            {
                lock (_obj)
                {
                    if (null == _instance)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                    return _instance;
                }
            }
        }

    }

    /// <summary>
    /// 基于MonoBehavior的单利类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GSingletonMono<T> :MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    // Object.DontDestroyOnLoad(go);  // 持久化
                }
                return _instance;
            }
        }

        public static T CreateFromPrefab(string path)
        {
            return CreateFromPrefab(Resources.Load<GameObject>(path));
        }
        
        public static T CreateFromPrefab(GameObject prefab)
        {
            if (prefab != null)
            {
                _instance = Instantiate(prefab).GetComponent<T>();
                return _instance;
            }
            return null;
        }

        public static T Attach(GameObject host)
        {
            _instance = host.AddComponent<T>();
            return _instance;
        }


        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            gameObject.name = typeof(T).Name;
        }
    }
    
    
}