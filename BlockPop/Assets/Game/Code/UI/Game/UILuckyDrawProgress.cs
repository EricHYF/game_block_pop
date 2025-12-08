using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    
    /// <summary>
    /// 幸运抽奖进度条
    /// </summary>
    public class UILuckyDrawProgress: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected Image progressBar;
		protected Button btnWithdraw;
		protected Text txtProgress;
		protected TextMeshProUGUI txtInfo;
		protected RectTransform hint;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			progressBar = FindUI<Image>("progress_bar");
			btnWithdraw = FindUI<Button>("btn_withdraw");
			txtProgress = FindUI<Text>("txt_progress");
			txtInfo = FindUI<TextMeshProUGUI>("txt_info");
			hint = FindUI<RectTransform>("hint");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}

		private Vector3 _hintStartPos;
		private bool _isFull;
		
		#region 初始化

		protected override void Init()
		{
			base.Init();
			_hintStartPos = hint.localPosition;
			hint.gameObject.SetActive(false);
			AddButton(btnWithdraw, OnClickWithdraw);
		}


		private void OnClickWithdraw()
		{
			// if (Vendor.IsLuckyDrawUseLocal)
			// {
			// 	Debug.Log($"----> 点击抽奖 | 已满:{_isFull} | 可抽:{HasLucky}");
			// 	if (HasLucky)
			// 	{
			// 		Vendor.GetNewLuckyDrawConfig(OnGetLuckDrawConfig);
			// 	}
			// }
			// else
			// {
			// 	Vendor.OpenWithdraw(); // 打开系统提现面板
			// }
			
			// Vendor.OpenWithdraw(); // 打开系统提现面板
			/*
			 * - 达成抽奖要求，进度条卡住，点击转盘弹出抽奖弹窗，如下图所示
			 * -  达到单个抽奖要求，抽奖完成后下方刷新进度条；Debug
			 * -  达到多个抽奖要求，首个抽奖完成后，刷新成下一个抽奖要求；
			 */
		}
		
		/// <summary>
		/// 获取抽奖奖励
		/// </summary>
		/// <param name="drawList"></param>
		private void OnGetLuckDrawConfig(LuckydrawData[] drawList)
		{
			// if (drawList != null && drawList.Length > 0)
			// {
			// 	UI.OpenLuckyDraw(drawList, Vendor.LuckyData);
			// }
		}

		#endregion

		#region 信息更新

		// private bool HasLucky => !string.IsNullOrEmpty(Vendor.LuckyData);
		
		
		/// <summary>
		/// 更新游戏抽奖进度
		/// </summary>
		public void UpdateInfo()
		{
			// if (Vendor.IsOpenWithdrawTip)
			// {
			// 	UpdateProgressWithSCCCount(); // 0.3元提现优化
			// }
			// else
			// {
			// 	UpdateProgressWithScore(); // 使用分数系统
			// }

			SetHint(_isFull);
		}
		
		/// <summary>
		/// 设置进度条显示
		/// </summary>
		/// <param name="value"></param>
		/// <param name="immediately"></param>
		private void SetProgressBar(float value, bool immediately = false)
		{
			var p = Mathf.Clamp01(value);
			float time = 0.3f;
			DOTween.Kill(progressBar);
			if (immediately)
			{
				progressBar.fillAmount = p;
			}
			else
			{
				progressBar.DOFillAmount(p, time);
			}
		}
		

		/// <summary>
		/// 用分数来显示进度
		/// </summary>
		private void UpdateProgressWithScore()
		{
			// var total = GameConfig.Instance.luckydrawTotalScore;
			// var score = Mathf.Min(User.UserData.luckydrawScore, total);
			/*
			var score = Vendor.GetScore();
			var total = Vendor.GetNextDrawScore();
			var isFull = HasLucky;

			if (isFull && _isFull == isFull) return;  // 满槽只显示一次
			
			bool immediately = _isFull != isFull;
			_isFull = isFull;
			
			txtProgress.text = $"{score}/{total}";
			txtInfo.text = $"再过<color=#FB7E3D> {total - score} </color>关, 即可获得额外<color=#FFFF00>提现</color>机会";
			if (_isFull)
			{
				txtInfo.text = $"获得额外抽奖，立即<color=#FFFF00>提现</color>";
				txtProgress.text = "可抽奖";
				SetProgressBar(1, true);
			}
			else
			{
				SetProgressBar((float) score / total, immediately);
			}
			*/
		}

		/// <summary>
		/// 使用完成数量显示进度
		/// </summary>
		private void UpdateProgressWithSCCCount()
		{
			/*
			var num = Vendor.SUCCCount;
			var total = Vendor.Withdraw3Limit;
			bool isFull = (num >= total && Vendor.HasWithdraw3);
			bool immediately = _isFull != isFull;
			_isFull = isFull;
			
			if (num < total)
			{
				//如果当前通关数小于可提现0.3需要的通关数时
				txtProgress.text = $"{num}/{total}";
				txtInfo.text = $"再过 <color=#FFFF00>{total - num}</color> 关，即可提现 <color=#FFFF00>{Vendor.FirstWithdrawPrice}</color> 元";
				SetProgressBar( (float) num / total, immediately);
			}
			else if(isFull)
			{
				//如果通关数大于可提现0.3需要的通关数并且0.3元是可提现状态时
				txtProgress.text = $"可提现";
				txtInfo.text = $"目标达成，可立即提现<color=#FFFF00>{Vendor.FirstWithdrawPrice}</color>元";
				SetProgressBar(1, true);
			}
			else
			{
				// 显示抽奖逻辑
				UpdateProgressWithScore();
			}


			*/
		}
		
		

		#endregion

		#region 提示效果

		
		/// <summary>
		/// 设置提现引导标志
		/// </summary>
		/// <param name="flag"></param>
		public void SetHint(bool active)
		{
			if (active)
			{
				if (!hint.gameObject.activeSelf)
				{
					hint.gameObject.SetActive(true);
					hint.localPosition = _hintStartPos;
					var toY = _hintStartPos.y + 40;
					hint.DOLocalMoveY(toY, 0.5f).SetLoops(-1, LoopType.Yoyo);
				}
			}
			else
			{
				DOTween.Kill(hint);
				hint.gameObject.SetActive(false);
			}
		}

		#endregion
		
    }
}
