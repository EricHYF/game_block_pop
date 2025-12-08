using System;
using DG.Tweening;
using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class UILevelInfo: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected TextMeshProUGUI txtTitleGoal;
		protected TextMeshProUGUI txtTitleLevel;
		protected RectTransform tips;
		protected TextMeshProUGUI txtTips;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			txtTitleGoal = FindUI<TextMeshProUGUI>("txt_title_goal");
			txtTitleLevel = FindUI<TextMeshProUGUI>("txt_title_level");
			tips = FindUI<RectTransform>("tips");
			txtTips = FindUI<TextMeshProUGUI>("txt_tips");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}


		private const float INFO_OFFSET = 900;
		
		
		#region 初始化

		public void OpenWithData(int level, int goal, Action callback)
		{
			txtTitleLevel.text = $"第{level}关";
			txtTitleGoal.text = $"目标：{goal}";
			root.localPosition = new Vector3(INFO_OFFSET, 0, 0);
			float time = 1.5f;

			Sound.Play(Sound.SFX.FXStart);
			
			DOTween.Sequence().Append(root.DOLocalMoveX(0, 0.3f))
				.AppendInterval(1f)
				.Append(root.DOLocalMoveX(-INFO_OFFSET,  0.3f))
				.SetAutoKill(true)
				.OnComplete(() =>
				{
					callback?.Invoke();
				}).Play();

			// var nextLevel = Vendor.Withdraw3Limit - Vendor.SUCCCount;
			//
			// if (Vendor.IsOpenWithdrawTip && nextLevel > 0)
			// {
			// 	tips.gameObject.SetActive(true);
			// 	txtTips.text = $"再过 <color=#FB7E3D>{nextLevel}</color> 关，可提现 <color=#FB7E3D>{Vendor.FirstWithdrawPrice}</color> 元";
			// }
			// else
			// {
			// 	tips.gameObject.SetActive(false);
			// }
		}
		


		#endregion
		
    }
}
