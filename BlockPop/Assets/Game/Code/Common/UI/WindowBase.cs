using System;
using DG.Tweening;
using GKIT.UI;

namespace Game
{
    public partial class WindowBase : ViewBase
    {

        public Action BeforeOpenHandle;
        public Action FinishOpenHandle;
        public Action BeforeCloseHandle;
        public Action FinishCloseHandle;

        /// <summary>
        /// 打开窗口
        /// </summary>
        public void Open()
        {
            OnBeforeOpen();
            // 执行打开的动画逻辑
            OnOpen();
        }

        protected virtual void OnBeforeOpen()
        {
            BeforeOpenHandle?.Invoke();
        }

        protected virtual void OnOpen()
        {
            OnFinishOpen();
        }

        protected virtual void OnFinishOpen()
        {
            FinishOpenHandle?.Invoke();
        }

        public void Close()
        {
            OnBeforeClose();
            BeforeCloseHandle?.Invoke();
            OnClose();
        }

        protected virtual void OnBeforeClose()
        {
            
        }

        protected virtual void OnClose()
        {
            PlaySound(Sound.SFX.UIWindowClose);
            if (_canvasGroup != null)
            {
                AddDisableRef();
                float time = 0.5f;
                _canvasGroup.DOFade(0, time).OnComplete(() =>
                {
                    SubDisableRef();
                    OnFinishClose();
                });
            }
            else
            {
                OnFinishClose();
            }
            
            
        }

        protected virtual void OnFinishClose()
        {
            UI.CloseWindow(this); // 上报关闭事件
            FinishCloseHandle?.Invoke();
            Dispose();
            Destroy(GameObject);
        }

        #region 动画事件

        protected override void OnAnimEvent(string eventName)
        {
            base.OnAnimEvent(eventName);
            
            
        }

        #endregion
       
    }
}