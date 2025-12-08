using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace GKIT
{
    /// <summary>
    /// 资源缓存库
    /// </summary>
    public class GResMap
    {
        public string name;
        public AsyncOperationHandle<IList<Object>> handle;
        public List<Object> objects;
        public GResMap(string key, AsyncOperationHandle<IList<Object>> ao)
        {
            name = key;
            handle = ao;
            if (null != handle.Result)
            {
                objects = handle.Result.ToList();
            }
            else
            {
                objects = new List<Object>();
            }
        }

        public T Find<T>(string name) where T : Object
        {
            return (T)objects.Find(c => c.name == name && (T)c != null);
        }

        public void Dispose()
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }
  
}