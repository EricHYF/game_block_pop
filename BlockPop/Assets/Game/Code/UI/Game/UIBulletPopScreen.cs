using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    
    /// <summary>
    /// 新版的Bullet弹幕
    /// </summary>
    public class UIBulletPopScreen: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected CanvasGroup toast;
		protected Image userHead;
		protected Text txtUserName;
		protected Text txtMoney;
		protected Text txtInfo;
		protected Text txtDate;
		protected UIBulletHead bulletHead;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			toast = FindUI<CanvasGroup>("toast");
			userHead = FindUI<Image>("user_head");
			txtUserName = FindUI<Text>("txt_user_name");
			txtMoney = FindUI<Text>("txt_money");
			txtInfo = FindUI<Text>("txt_info");
			txtDate = FindUI<Text>("txt_date");
			bulletHead = FindUI<UIBulletHead>("bullet_head");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}


		#region 属性定义


		private Vector2 _intervalRange;
		private float _interval;
		private float _duration;
		private bool _onShowing;
		

		#endregion
		
		
		
		#region 初始化

		/// <summary>
		/// 初始化
		/// </summary>
		protected override void Init()
		{
			base.Init();
			_intervalRange = new Vector2(10, 20);
			_duration = 2;


		}

		#endregion

		#region 显示接口

		private Coroutine _coroutine;
		
		/// <summary>
		/// 显示弹幕
		/// </summary>
		public void Show()
		{
			if (_onShowing) return;
			_onShowing = true;
			if(!Active) Active = true;
			_coroutine = StartCoroutine(OnScreenShowing());
		}

		/// <summary>
		/// 隐藏弹幕
		/// </summary>
		public void Hide()
		{
			ResetNode();
			_onShowing = false;
			if(_coroutine != null)StopCoroutine(_coroutine);
		}


		private void ResetNode()
		{
			toast.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 180, 0);
			toast.alpha = 1;
		}


		/// <summary>
		/// 显示
		/// </summary>
		/// <returns></returns>
		private IEnumerator OnScreenShowing()
		{
			_onShowing = true;
			ResetNode();
			
			yield return new WaitForSeconds(_intervalRange.x);
			
			while (_onShowing)
			{
				UpdateInfo();

				float time = 0.5f;
 
				ResetNode();
				// float toY = -80f;
				// toast.GetComponent<RectTransform>().DOAnchorPosY(toY, time);
				toast.transform.DOLocalMoveY(0, time);

				yield return new WaitForSeconds(time+ _duration);

				toast.DOFade(0, time);
				
				yield return new WaitForSeconds(_interval);
			}

			yield return new WaitForSeconds(0.1f);
		}

		/// <summary>
		/// 获取下一次显示的信息
		/// </summary>
		private void UpdateInfo()
		{
			string name = GetRandName();
			string money = GetRandValue();
			var now = DateTime.Now;

			_interval = Random.Range(_intervalRange.x, _intervalRange.x);

			txtUserName.text = name;
			txtMoney.text = $"{money}元";
			txtInfo.text = $"提现了{money}元，微信打款已到账";
			txtDate.text = $"{now.Year}年{now.Month}月{now.Day}日";
			
			bulletHead.Init();
		}


		#endregion


		#region 数据准备

		private string GetRandName()
		{
			var n = Random.Range(1001, 9999);
			return $"用户1***{n}";
		}

		private string GetRandValue()
		{
			var r = Random.Range(0, 100);
			if(r <= 2 )
			{
				return "2000";
			}
			else if (r <= 5)
			{
				return "1000";
			}
			else if (r <= 60)
			{
				return "500";
			}
		
			return "300";
			
		}
		
		

		#endregion
        
        
        
        
        
        
    }
}
