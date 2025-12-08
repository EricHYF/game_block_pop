using GKIT.Pool;
using GKIT.UI;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace GKIT
{
    /// <summary>
    /// UI元素工厂
    /// </summary>
    public class UIElementFactory : UnityComponentFactoryBase<UIBinder>
    {
        
        #region 静态接口
        /// <summary>
        /// 加载UI
        /// </summary>
        /// <param name="address"></param>
        /// <param name="callback"></param>
        /// <param name="parent"></param>
        public static void Load<T> (string address, Action<T> callback, Transform parent = null) where T : UIElementBase
        {
            GRes.CloneAsync(address, parent).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    handle.Result.name = address.Substring(address.LastIndexOf("/", StringComparison.Ordinal));
                    
                    var t = Activator.CreateInstance<T>();
                    t.BindView(handle.Result.transform);
                    callback?.Invoke(t);
                }
                else
                {
                    callback?.Invoke(null);
                }
            };
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="address"></param>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static AsyncOperationHandle<GameObject> LoadAsync<T>(string address, Transform parent = null)
        {
            return GRes.CloneAsync(address, parent);
        }
        
        #endregion

        #region ObjectPoolFactory

        private GameObject _prefab;

        public void SetPrefab(GameObject prefab)
        {
            if (prefab.transform.TryGetComponent(out UIBinder binder))
            {
                _prefab = prefab;
            }
        }
        
        
        protected override UIBinder Create()
        {
            if (null == _prefab)
                throw new Exception("You need call [SetPrefab] method first to set the gameObject source.");

            GameObject go = Object.Instantiate(_prefab);
            return go.GetComponent<UIBinder>();
        }

        public override void Reset(UIBinder target)
        {
            target.StopAllCoroutines();
            target.gameObject.SetActive(false);
        }
        

        #endregion
        
    }


}