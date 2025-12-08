using TMPro;
using System;
using DG.Tweening;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    
    /// <summary>
    /// 弹幕组件
    /// </summary>
    public class UIBulletScreenItem: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected Text txtInfo;
		protected UIBulletHead bulletHead;
		protected Image userHead;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			txtInfo = FindUI<Text>("txt_info");
			bulletHead = FindUI<UIBulletHead>("bullet_head");
			userHead = FindUI<Image>("user_head");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}

		private Vector3 _startPos;
		private float _moveTime;
		private Action<UIBulletScreenItem> _onComplete;
		
		#region 生命周期

		protected override void Init()
		{
			base.Init();
			
		}


		public void InitWithData(float moveTime, Action<UIBulletScreenItem> callback)
		{
			_moveTime = moveTime;
			_onComplete = callback;
		}

	
		/// <summary>
		/// 重置移动信息
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="startPos"></param>
		public void ResetMoveInfo(string name, string value, Vector3 startPos)
		{
			txtInfo.text = $"恭喜<color=#2057A0>{name}</color>提现<color=#FF0000>{value}元</color>";
			_startPos = startPos;
			Active = true;
			bulletHead.Init();
			ResetMove();
		}


		private void ResetMove()
		{
			var pos = _startPos;
			pos.x += 200;
			Transform.localPosition = pos;
			Transform.DOLocalMoveX(-pos.x, _moveTime)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					_onComplete?.Invoke(this);
				});
		}


		public void OnSaved()
		{
			Transform.DOKill();
			Active = false;
		}
		
		

		#endregion
		
    }
}
