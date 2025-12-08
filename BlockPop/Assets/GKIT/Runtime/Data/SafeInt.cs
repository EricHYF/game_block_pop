using System;
using System.Net.Http.Headers;
using Random = UnityEngine.Random;

namespace GKIT
{
    public class SafeInt : Property<int>
    {
        
        private int _offset;
        private int _safeValue;

        public SafeInt(int defValue) : base(defValue)
        {
            _offset = Random.Range(-10000, 0);  // 生成偏移量
            Value = defValue;
        }
        
        
        protected override void SetValue(int value)
        {
            _safeValue = -value + _offset;
        }

        protected override int GetValue()
        {
            return  -_safeValue + _offset;
        }
    }

}