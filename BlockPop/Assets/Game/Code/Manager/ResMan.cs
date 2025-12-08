using System;
using System.Collections.Generic;
using GKIT;
using GKIT.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Game
{
    public class ResMan: GSingleton<ResMan>
    {

        private Dictionary<string, GResMap> maps;
        
        /// <summary>
        /// 需要预加载的包
        /// </summary>
        private static string[] preloadKeys = new string[]
        {
            // "common",
            "game",
            "ui",
            "sounds",
        };



        public static async void StartPreLoad(Action callback, Action<float> onLoading = null)
        {
            int p = 0;
            int total = preloadKeys.Length;
            Instance.maps = new Dictionary<string, GResMap>(preloadKeys.Length);
            foreach (var key in preloadKeys)
            {
                var handle = Addressables.LoadAssetsAsync<Object>(key, res => { });
                await handle.Task;
            
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var lib = new GResMap(key, handle);
                    Instance.maps[key] =  lib;
                }
                // Addressables.Release(handle);
                p++;
                onLoading?.Invoke((float)p/total);
            }
            onLoading?.Invoke(1);
            callback?.Invoke();
        }


        public static bool MapExists(string key)
        {
            return Instance.maps.ContainsKey(key);
        }

        public static T AddressLoad<T>(string address) where T : Object
        {
            AddressToKeyAndName(address, out var key, out var name);
            return LoadExits<T>(key, name);
        }
        
        /// <summary>
        /// 加载已有资源
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadExits<T>(string key, string name) where T : Object
        {
            if (MapExists(key))
            {
                return Instance.maps[key].Find<T>(name);
            }
            return default;
        }
        
        /// <summary>
        /// Resources加载
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResourcesLoad<T>(string name) where T : Object
        {
            return Resources.Load<T>(name);
        }
        
        
        
        

        /// <summary>
        /// 示例化对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject Clone(string key, string name, Transform parent = null)
        {
            GameObject prefab = LoadExits<GameObject>(key, name);
            if (null != prefab)
            {
                GameObject go = Object.Instantiate(prefab, parent);
                go.name = name;
                return go;
            }
            return null;
        }

        public static void AddressToKeyAndName(string address, out string key, out string name)
        {
            key = "";
            name = "";
            int start = 0;
            int end = address.IndexOf("/", StringComparison.Ordinal);
            key = address.Substring(start, end);
            start = address.LastIndexOf("/", StringComparison.Ordinal) + 1;
            end = address.Length - start;
            name = address.Substring(start, end);
        }
        
        public static GameObject Clone(string address, Transform parent = null)
        {
            AddressToKeyAndName(address, out var key, out var name);
            return Clone(key, name, parent);
        }

        public static void Destroy(GameObject target)
        {
            Addressables.ReleaseInstance(target);
        }

        public static T CreateUI<T>(string address, Transform parent = null) where T : UIElementBase
        {
            GameObject go = Clone(address, parent);
            if (null != go)
            {
                T ui = Activator.CreateInstance<T>();
                ui.BindView(go);
                return ui;
            }
            Debug.Log($"<color=orange>--- Create UI Fail ---</color>");
            return null;
        }
        
        /// <summary>
        /// 加载SpriteAtlas
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static SpriteAtlas LoadSpriteAtlas(string address)
        {
            AddressToKeyAndName(address, out var key, out var name);
            return LoadExits<SpriteAtlas>(key, name);
        }


        public static GameObject LoadGameObject(string address)
        {
            AddressToKeyAndName(address, out var key, out var name);
            return LoadExits<GameObject>(key, name);
        }

    }
    
    





}