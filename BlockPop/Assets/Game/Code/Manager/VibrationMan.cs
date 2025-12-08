using System.Collections;
using System.Collections.Generic;
using GKIT;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Game
{
    public class VibrationMan : GSingleton<VibrationMan>
    {
        public const string K_VIBRATION_ENABLE_FLAG = "vibration_enable_flag";



        private bool _enableVibration;
        
        public bool VibrationEnabled
        {
            get => _enableVibration;
            set => SetVibrationEnabled(value);
        }



        public void Init()
        {
            GetVibrationEnabled();
        }
        

        public void Haptic (HapticTypes type, bool defaultToRegularVibrate = false, bool alsoRumble = false, MonoBehaviour coroutineSupport = null, int controllerID = -1)
        {
            if (!VibrationEnabled) return;
            MMVibrationManager.Haptic(type, defaultToRegularVibrate, alsoRumble, coroutineSupport, controllerID);
        }

        #region 开关管理

        private void GetVibrationEnabled()
        {
            if (!PlayerPrefs.HasKey(K_VIBRATION_ENABLE_FLAG))
            {
                SetVibrationEnabled(true);
                return;
            }
            _enableVibration = PlayerPrefs.GetInt(K_VIBRATION_ENABLE_FLAG, 0) == 1;
        }

        private void SetVibrationEnabled(bool value)
        {
            _enableVibration = value;
            PlayerPrefs.SetInt(K_VIBRATION_ENABLE_FLAG, value? 1 : 0);
        }

        #endregion

    }
}