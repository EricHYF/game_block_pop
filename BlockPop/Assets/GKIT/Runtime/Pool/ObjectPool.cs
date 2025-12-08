using System;
using UnityEngine;

namespace GKIT.Pool
{
    
    public class ObjectPool<T> : IObjectPool<T> where T : class
    {

        private int _initialSize;
        private int _maxSize;
        private IObjectFactory<T> _factory;
        private Entry[] _entries;
        private bool _disposed = false;
        
        public ObjectPool(IObjectFactory<T> factory) : this(factory, 0, Environment.ProcessorCount * 2)
        {
        }
        
        public ObjectPool(IObjectFactory<T> factory, int maxSize) : this(factory, 0, maxSize)
        {
        }
        
        public ObjectPool(IObjectFactory<T> factory, int initialSize, int maxSize)
        {
            _factory = factory;
            _initialSize = initialSize;
            _maxSize = maxSize;
            _entries = new Entry[maxSize];
            
            if (maxSize < initialSize)
                throw new ArgumentException("the maxSize must be greater than or equal to the initialSize");

            for (int i = 0; i < initialSize; i++)
            {
                _entries[i].value = factory.Create(this);
            }

        }
        
        public int MaxSize => _maxSize;
        public int InitialSize => _initialSize;

        /// <summary>
        /// 获取对象池内的对象
        /// </summary>
        /// <returns></returns>
        public T Fetch()
        {
            var value = default(T);
            for (int i = 0; i < _entries.Length; i++)
            {
                value = _entries[i].value;
                if(value == null) 
                    continue;
                
                _entries[i].value = null;
                return value;
            }
            return _factory.Create(this);
        }

        public void Free(T obj)
        {
            if (obj == null)
                return;
            if (_disposed || !_factory.Validate(obj))
            {
                _factory.Destroy(obj);
                return;
            }
            
            _factory.Reset(obj);
            for (int i = 0; i < _entries.Length; i++)
            {
                if (null == _entries[i].value)
                {
                    _entries[i].value = obj;
                    return;
                }
            }
            _factory.Destroy(obj);
        }


        protected virtual void Clear()
        {
            for (var i = 0; i < _entries.Length; i++)
            {
                var value = _entries[i].value;
                _entries[i].value = null;
                
                if(null != value)
                    _factory.Destroy(value);
            }
        }

        #region Disposing

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                this.Clear();
                _disposed = true;
            }
        }
        
        ~ObjectPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        private struct Entry
        {
            public T value;
        }
        
    }
}