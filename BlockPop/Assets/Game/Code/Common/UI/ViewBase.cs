using System;
using GKIT.UI;
using MoreMountains.NiceVibrations;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class ViewBase : UIContainerBase
    {
        
        #region 管理器引用

        public UserMan User => UserMan.Instance;
        public UIMan UI => UIMan.Instance;
        public SoundMan Sound => SoundMan.Instance;
        public VibrationMan Vibration => VibrationMan.Instance;
        // public Vendor Vendor => Vendor.Instance;
        // public RedPacketMan RedPacket => RedPacketMan.Instance;

        #endregion

        #region 事件管理
        
        public void AddEvent(Events eventsKey, Action<object> callback)
        {
            base.AddEvent(eventsKey, callback);
        }

        public void RemoveEvent(Events eventsKey, Action<object> callback)
        {
            base.RemoveEvent(eventsKey, callback);
        }

        public void SendEvent(Events eventsKey, object data = null)
        {
            base.SendEvent(eventsKey, data);
        }

        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="callback"></param>
        public void AddButton(Button btn, UnityAction callback)
        {
            btn.onClick.AddListener(callback);
        }

        public void RemoveButton(Button btn, UnityAction callback)
        {
            btn.onClick.RemoveListener(callback);
        }

        #endregion
        
        #region 音效
        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="key"></param>
        public void PlaySound(string key)
        {
            Sound.Play(key);
        }
        
        /// <summary>
        /// 播放按钮音效
        /// </summary>
        /// <param name="key"></param>
        public void PlayButtonSound(string key = "")
        {
            if (string.IsNullOrEmpty(key)) key = Sound.SFX.UIButtonClick;
            Sound.Play(key);
        }
        

        #endregion

        #region 震动

        /// <summary>
        /// 震动
        /// </summary>
        /// <param name="types"></param>
        public void Haptic(HapticTypes types)
        {
            Vibration.Haptic(types);
        }


        #endregion
        
    }
}