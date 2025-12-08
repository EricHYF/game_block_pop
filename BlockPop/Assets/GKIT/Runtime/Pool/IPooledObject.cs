namespace GKIT.Pool
{
    public interface IPooledObject
    {
        /// <summary>
        /// Return the object to the pool.
        /// </summary>
        void Free();
    }
}