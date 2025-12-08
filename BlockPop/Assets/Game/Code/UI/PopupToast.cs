using System;
using DG.Tweening;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class PopupToast : PopupBase
    {
        //********** [CODE GEN] PROPERTY DEFINE HEAD **********//
        protected RectTransform root;
        protected RectTransform content;
        protected Text label;
        protected CanvasGroup canvasGroup;
        //********** [CODE GEN] PROPERTY DEFINE END **********//
        protected override void OnCreateOver()
        {
            //********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
            base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
            root = FindUI<RectTransform>("root");
            content = FindUI<RectTransform>("content");
            label = FindUI<Text>("label");
            canvasGroup = FindUI<CanvasGroup>("canvas_group");
            //********** [CODE GEN] PROPERTY STATEMENT END **********//
        }
		
        /// <summary>
        ///  显示Popup
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startY"></param>
        /// <param name="callback"></param>
        /// <param name="delay"></param>
        public void Show(string content,float startY, Action callback = null, float delay = 0)
        {
            Active = true;
            float endY = startY + 100;
            canvasGroup.alpha = 0;
            canvasGroup.transform.localPosition = new Vector3(0, startY, 0);
            label.text = content;
				
            float time = 1f;
            canvasGroup.transform.DOLocalMoveY(endY, time);
            canvasGroup.DOFade(1, time);
            canvasGroup.DOFade(0, time * 0.5f).SetDelay(time + time * 0.5f).OnStepComplete(() =>
            {
                // OnRelease();
                Close();
            });
        }
		
		
		
		
        
    }
}