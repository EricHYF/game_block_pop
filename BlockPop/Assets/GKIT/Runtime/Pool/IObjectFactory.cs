namespace GKIT.Pool
{
    /// <summary>
    /// 对象池工厂类, 用于生成特定的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectFactory<T> where T : class
    {
        /// <summary>
        /// 创建一个 <typeparamref name="T"/>
        /// </summary>
        /// <param name="pool"></param>
        /// <returns></returns>
        T Create(IObjectPool<T> pool);
        
        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="obj"></param>
        void Destroy(T obj);
        
        /// <summary>
        /// 重置对象
        /// </summary>
        /// <param name="target"></param>
        void Reset(T target);
        
        /// <summary>
        /// 验证对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Validate(T obj);
    }
}