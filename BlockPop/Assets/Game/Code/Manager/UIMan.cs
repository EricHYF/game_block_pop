using TMPro;
using GKIT;
using System;
using System.Collections.Generic;
using DG.Tweening;
using GKIT.UI;
using UnityEngine.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{

    public enum UILayer
    {
        Content,        // 一般内容层a
        Popup,          // 弹窗层
        Toast,          // 消息层
        Top,                // 顶层
        Tutorial,           // 教学层
    }
    
    
    public partial class  UIMan : ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected Camera uiCamera;
		protected RectTransform root;
		protected RectTransform content;
		protected RectTransform popup;
		protected RectTransform toast;
		protected RectTransform top;
		protected RectTransform tutorial;
		protected RectTransform loading;
		protected Image loadingBar;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			uiCamera = FindUI<Camera>("ui_camera");
			root = FindUI<RectTransform>("root");
			content = FindUI<RectTransform>("content");
			popup = FindUI<RectTransform>("popup");
			toast = FindUI<RectTransform>("toast");
			top = FindUI<RectTransform>("top");
			tutorial = FindUI<RectTransform>("tutorial");
			loading = FindUI<RectTransform>("loading");
			loadingBar = FindUI<Image>("loading_bar");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}


		private Canvas _canvas;
		private CanvasScaler _canvasScaler;
		public Camera UICamera => uiCamera;
		public Canvas UICanvas => _canvas;
		
		private static UIMan _instance;
		public static UIMan Instance
        {
	        get
	        {
		        if (null == _instance)
		        {
			        _instance = new UIMan();
			        string name = "UIRoot";
			        GameObject go = Object.Instantiate(Resources.Load<GameObject>(name));
			        go.name = name;
			        _instance.BindView(go);
		        }
		        return _instance; 
	        }
        }
		
		private WindowBase _curWindow;
		private HashSet<PopupBase> _popups;
		private HashSet<WindowBase> _windows;
		private ScreenFitter _screenFitter;


		
        public ScreenFitter Fitter => _screenFitter;  //适配器

        public Vector2 ScreenSize => new Vector2(Screen.width, Screen.height);
        
        public Vector2 FitScreenSize =>  ScreenSize * Fitter.UIScale;

        public float UIScale => Fitter.UIScale;
        /// <summary>
        /// 根节点尺寸
        /// </summary>
        public Vector2 Size => RectTransform.sizeDelta;
        private Action _rewardCallback;
        /// <summary>
        /// 判断是否是Pad
        /// </summary>
        public bool IsPad => _screenFitter.IsPad;
        /// <summary>
        /// 是否是刘海屏
        /// </summary>
        public bool IsLongScreen => _screenFitter.IsLongScreen;

        public float ScreenRatio => _screenFitter.ScreenRatio;


		/// <summary>
		/// 奖励动画播放完的回调
		/// </summary>
		public Action RewardCallback
		{
			set => this._rewardCallback = value;
			get => this._rewardCallback;
		}
		
		public bool IsPopupShowing => _popups.Count > 0;


		#region 初始化

		protected override void Init()
        {
	        base.Init();
	        Transform.TryGetComponent(out _canvas);
	        Transform.TryGetComponent(out _canvasScaler);
	        Object.DontDestroyOnLoad(GameObject);

	        _popups = new HashSet<PopupBase>(); // 弹窗列表
	        _windows = new HashSet<WindowBase>(); // 主窗体列表
	        _screenFitter = new ScreenFitter();
	        _screenFitter.Init(root);

	        // 时间初始化
	        AddEvents();
        }

        #endregion

        #region UI 管理
        
        public static Transform GetRootNode(UILayer layer = UILayer.Content)
        {
	        switch (layer)
	        {
		        case UILayer.Popup: return Instance.popup;
		        case UILayer.Toast: return Instance.toast;
		        case UILayer.Top: return Instance.top;
		        case UILayer.Tutorial: return Instance.tutorial;
		        default: return Instance.content;
	        }
        }
        

        
        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <param name="address"></param>
        /// <param name="layer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateUI<T>(string address, Transform parent) where T : UIContainerBase
        {
			return ResMan.CreateUI<T>(address, parent);
        }


        public T OpenWindow<T>(string address, UILayer layer = UILayer.Content, bool autoOpen = true) where T : WindowBase
        {
	        var window = AddView<T>(address, layer);
	        _windows.Add(window);
	        if(autoOpen) window.Open();
	        SendEvent(Events.OpenWindow, window);
	        return window;
        }
        
        /// <summary>
        /// 查找窗体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindWindow<T>(string name = "") where T : WindowBase
        {		
	        foreach (var win in _windows)
	        {
		        if (string.IsNullOrEmpty(name))
		        {
			        if (win.GetType() == typeof(T))
			        {
				        return (T)win;
			        }
		        }
		        else
		        {
			        if (win.Name == name)
			        {
				        return (T)win;
			        }
		        }
		        
	        }
	        return null;
        }


        public void CloseWindow<T>(T window) where T : WindowBase
        {
	        PopupBase pop = window as PopupBase;
	        if (pop != null)
	        {
		        if (_popups.Contains(pop)) _popups.Remove(pop);
	        }
	        else
	        {
		        if (_windows.Contains(window)) _windows.Remove(window);
	        }
        }
        
        
		
        /// <summary>
        /// 打开弹窗
        /// </summary>
        /// <param name="address"></param>
        /// <param name="autoOpen"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T OpenPopup<T>(string address, bool autoOpen = true) where T : PopupBase
        {
	        var pop = AddView<T>(address, UILayer.Popup);
	        if(autoOpen) pop.Open();
	        _popups.Add(pop);
	        SendEvent(Events.OpenPopup, pop);
	        return pop;
        }

        public T FindPopup<T>(string name = "") where T : PopupBase
        {		
	        foreach (var pop in _popups)
	        {

		        if (string.IsNullOrEmpty(name))
		        {
			        if (pop.GetType() == typeof(T))
			        {
				        return (T)pop;
			        }
		        }
		        else
		        {
			        if (pop.Name == name)
			        {
				        return (T)pop;
			        }
		        }
	        }
	        return null;
        }


        public T AddView<T>(string address, UILayer layer = UILayer.Content) where T : ViewBase
        {
	        var view = CreateUI<T>(address, GetRootNode(layer));
	        return view;
        }
        


        public void DisplayLoading(bool active = true)
        {
	      
        }

        public void DisplayBuyLoading(bool enabled = true)
        {
	        
        }

        #endregion

        #region UI事件管理
		
        private void AddEvents()
        {
        }





        public void CloseAllWindows()
	    {
		    try
		    {
			    if (_windows != null && _windows.Count > 0)
			    {
				    foreach (var win in _windows)
				    {
					    if(win != null) win.Close();
				    }
				    _windows.Clear();
			    }
		    }
		    catch (Exception e)
		    {
			    Debug.LogError(e);
		    }
		    
	    }


	    public void CloseAllPopups()
	    {
		    if (_popups != null && _popups.Count > 0)
		    {
			    foreach (var pop in _popups)
			    {
				    pop.Close();
			    }
			    _popups.Clear();
		    }
	    }
        

        #endregion

        

        #region 对话框
		
        /// <summary>
        /// 显示小型对话框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ok"></param>
        /// <param name="onConfirm"></param>
        public void ShowDialogSmall(string title, string message, string ok = null, Action onConfirm = null)
        {
	        // var pop = OpenPopup<WindowDialogBase>(G.Address.DialogSmall);
	        // pop?.OpenWithMessage(title, message, ok, onConfirm);
        }
        
        
        

        #endregion
        


        #region 展示Toast
		
        /// <summary>
        /// 展示Toast
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startY"></param>
        /// <param name="delay"></param>
        /// <param name="callback"></param>
        public PopupToast Toast(string msg, float startY = 0, float delay = 0, Action callback = null)
        {
	        if (string.IsNullOrEmpty(msg))
	        {
		        callback?.Invoke();
		        return null;
	        }
	        var popup = OpenPopup<PopupToast>(G.Address.PopupToast);
	        popup.Show(msg, startY, callback, delay);
	        return popup;
        }

        #endregion
        
        #region Loading节点

        public void HideLoadNode()
        {
	        loading.gameObject.SetActive(false);
        }
        public void ShowLoadNode()
        {
	        SetLoadingProgress(0, true);
	        loading.gameObject.SetActive(true);
        }
        
        public void SetLoadingProgress(float progress, bool immediately = false)
        {
	        progress = Mathf.Clamp01(progress);
	        if (immediately)
	        {
		        loadingBar.fillAmount = progress;
	        }
	        else
	        {
		        loadingBar.DOFillAmount(progress, 0.3f).SetEase(Ease.Linear);

	        }
        }
        

        #endregion

    }
    
}
