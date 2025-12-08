using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace GKIT
{

    public enum ResourceLoadMode
    {
        Local,
        StreamingAssets,
        Online,
    }
    
    
    public static class GRes
    {
        public const string ResourceExt = "ResourceExt";

        private static ResourceLoadMode _loadMode;
        public static ResourceLoadMode LoadMode
        {
            get => _loadMode;
            set => _loadMode = value;
        }


        public static AsyncOperationHandle<long> GetDownloadSize(string key)
        {
            return Addressables.GetDownloadSizeAsync(key);
        }

        public static AsyncOperationHandle  DownloadDependenciesAsync(string key)
        {
            return Addressables.DownloadDependenciesAsync(key, true);
        }
        


        public static T Load<T>(string name, Action<T> onComplete = null)
        {
            Addressables.LoadAssetAsync<T>(name).Completed += (handler =>
            {
                onComplete?.Invoke(handler.Result);
            });

            return default;
        }

        public static AsyncOperationHandle<T> LoadAsync<T>(string name)
        {
            return Addressables.LoadAssetAsync<T>(name);
        }

        

        public static void Clone(string name,  Action<GameObject> onComplete = null, Transform parent = null)
        {
            Addressables.InstantiateAsync(name, parent).Completed += handler =>
            {
                onComplete?.Invoke(handler.Result);
                Addressables.Release(handler);
            };
        }

        public static AsyncOperationHandle<GameObject> CloneAsync(string name, Transform parent = null)
        {
            return Addressables.InstantiateAsync(name, parent);
        }
        
        

        public static bool Unload(GameObject gameObject)
        {
            return Addressables.ReleaseInstance(gameObject);
        }


        public static async void LoadAssetsAsync(string key, Action<bool, List<UnityEngine.Object>> callback)
        {
            var handle = Addressables.LoadAssetsAsync<UnityEngine.Object>(key, res => { });
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                callback?.Invoke(true, handle.Result.ToList());
            }
            else
            {
                callback?.Invoke(false, null);
            }
            Addressables.Release(handle);
        }
        
        
        /// <summary>
        /// 加资源套件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        public static async void LoadResMap(string key, Action<bool, GResMap> callback)
        {
            var handle = Addressables.LoadAssetsAsync<Object>(key, res => { });
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var lib = new GResMap(key, handle);
                lib.objects = handle.Result.ToList();
                callback?.Invoke(true, lib);
            }
            else
            {
                callback?.Invoke(false, null);
            }
        }
        
    }




    /// <summary>
    /// 资源路径类
    /// </summary>
    public class ResPath
    {
        
        
        
        
        private string _path;

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                Parse(_path);
            }
        }

        private string _packageName;
        private string _resName;
        public string Package
        {
            get
            {
                if(string.IsNullOrEmpty(_packageName)) Parse(_path);
                return _packageName;
            }
        }

        public string Name
        {
            get
            {
                if(string.IsNullOrEmpty(_resName)) Parse(_path);
                return _resName;
            }
        }

        public void Parse(string raw)
        {
            int idx = raw.IndexOf("/");
            if (idx > 0)
            {
                _packageName = raw.Substring(0, idx);
                _resName = raw.Substring(idx+1);
            }
        }

        public ResPath(string resourcePath = null)
        {
            Path = resourcePath;
        }
        
        
        
        
        
    }
    
    
}





