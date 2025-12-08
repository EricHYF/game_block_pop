using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UISimpleToggle: MonoBehaviour
    {
        [SerializeField] private GameObject _iconOn;
        [SerializeField] private GameObject _iconOff;
        [SerializeField] private Button _btn;
        
        public Action<bool> OnValueChanged;
        private bool _value;
        public bool Value
        {
            get => _value;
            set => SetValue(value);
        }


        private void Awake()
        {
            _btn.onClick.AddListener(() =>
            {
                Value = !Value;
            });
        }


        private void SetValue(bool value)
        {
            _value = value;
            _iconOn.SetActive(_value);
            _iconOff.SetActive(!_value);
            OnValueChanged?.Invoke(_value);
        }


    }
}