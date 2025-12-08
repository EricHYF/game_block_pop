using DG.Tweening;
using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    
    /// <summary>
    /// 缓冲红包(无奖励)
    /// </summary>
    public class PopupHuanChongMoneyFail:  PopupBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected Button btnClose;
		protected TextMeshProUGUI txtInfo;
		protected Image progressBar;
		protected Button btnContinue;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			btnClose = FindUI<Button>("btn_close");
			txtInfo = FindUI<TextMeshProUGUI>("txt_info");
			progressBar = FindUI<Image>("progress_bar");
			btnContinue = FindUI<Button>("btn_continue");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}

		#region 生命周期

		protected override void Init()
		{
			base.Init();
			txtInfo.text = $"已获得{User.UserMoney:F2}元";
			float pc = Mathf.Clamp(User.UserMoney / 300, 0, 0.97f);
			progressBar.fillAmount = pc;
			AddButton(btnContinue, OnClickContinue);
		}

		private void OnClickContinue()
		{
			Close();
		}


		protected override void OnOpen()
		{
			base.OnOpen();
			float time = 0.5f;
			root.localScale = Vector3.zero;
			root.DOScale(1, time).SetEase(Ease.OutBounce);
		}

		#endregion
        
    }
}
