using UnityEngine;

namespace GKIT.Pool
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPool<T> where T : class
    {
        /// <summary>
        /// 如果可能, 从对象池内获取对象, 否则就创建一个
        /// </summary>
        /// <returns></returns>
        T Fetch();
        
        /// <summary>
        /// 释放对象, 将对象归还对象池, 如果对象池数量已经超过最大值, 则销毁对象
        /// </summary>
        /// <param name="obj"></param>
        void Free(T obj);
        
        
    }
}