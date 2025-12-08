using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace GKIT
{
    public abstract class Property<T>
    {
        public event Action<T> OnValueChanged;

        protected T _value;
        public T Value
        {
            get => GetValue();
            set
            {
                SetValue(value);
                OnValueChanged?.Invoke(_value);
            }
        }

        public Property(T defValue = default)
        {
            _value = defValue;
        }

        protected abstract void SetValue(T value);

        protected abstract T GetValue();

    }
}