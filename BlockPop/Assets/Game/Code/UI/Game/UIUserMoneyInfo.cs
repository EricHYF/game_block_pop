using DG.Tweening;
using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class UIUserMoneyInfo: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected Text txtMoney;
		protected Button btnWithdraw;
		protected Image icon;
		protected Canvas canvasMoney;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			txtMoney = FindUI<Text>("txt_money");
			btnWithdraw = FindUI<Button>("btn_withdraw");
			icon = FindUI<Image>("icon");
			canvasMoney = FindUI<Canvas>("canvas_money");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}

		public Vector3 IconPos => icon.transform.position;
		
		
		#region 初始化

		protected override void Init()
		{
			base.Init();
			AddButton(btnWithdraw, OnClickWithdraw);
			// Vendor.GetUserMoney(UpdateUserMoney);
			canvasMoney.overrideSorting = false;
		}

		private void OnClickWithdraw()
		{
			// Vendor.OpenWithdraw();
		}
		
		#endregion

		#region 钱数更新

		private float _lastValue = 0;

		public void UpdateUserMoney(float value)
		{
			if (value > _lastValue)
			{
				float time = 1f;
				DOTween.To((m =>
				{
					txtMoney.text = $"{m:F2}元";
				}), _lastValue, value, time).OnComplete(() =>
				{
					_lastValue = value;
				});
			}
			txtMoney.text = $"{value:F2}元";
		}

		

		#endregion

		#region 钱数弹动

		private Sequence _scaleSeq;
		public void StartMoneyPop(float delay = 0)
		{
			canvasMoney.overrideSorting = true;
			txtMoney.transform.localScale = Vector3.one;
			_scaleSeq = DOTween.Sequence()
				.SetAutoKill(true)
				.SetDelay(delay)
				.Append(txtMoney.transform.DOScale(1.2f, 0.3f))
				.AppendInterval(1f)
				.Append(txtMoney.transform.DOScale(1f, 0.5f).OnComplete(() =>
				{
					canvasMoney.overrideSorting = false;
					_scaleSeq = null;
				})).Play();

			// _scaleTween = txtMoney.transform.DOScale(1.5f, time).SetLoops(1, LoopType.Yoyo);
		}

		public void StopMoneyPop()
		{
			_scaleSeq?.Kill();
			canvasMoney.overrideSorting = false;
			txtMoney.transform.localScale = Vector3.one;
		}


		#endregion

    }
}
