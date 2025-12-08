using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using JollyArt;
using Random = UnityEngine.Random;

namespace Game
{
    public class PopupLuckyDraw: PopupBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected Button btnPlay;
		protected RectTransform board;
		protected TextMeshProUGUI txtTime;
		protected RectTransform items;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			btnPlay = FindUI<Button>("btn_play");
			board = FindUI<RectTransform>("board");
			txtTime = FindUI<TextMeshProUGUI>("txt_time");
			items = FindUI<RectTransform>("items");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}


		private DateTime _luckDate;
		private Coroutine _countdown;
		private bool _onRolling;
		private LuckydrawResult _curResultData;

		protected override void Init()
		{
			base.Init();

			AddButton(btnPlay, OnClickPlay);
		}

		/// <summary>
		/// 初始化所有的旋转物件
		/// </summary>
		private void InitSpinItems(LuckydrawData[] drawList)
		{
			var spItems = this.items.GetComponentsInChildren<LuckyDrawSpinItem>();
			for (int i = 0; i < drawList.Length; i++)
			{
				if (i < spItems.Length)
				{
					var item = spItems[i];
					item.Init(drawList[i], i);
				}
			}
		}


		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="luckData"></param>
		public void InitWithData(LuckydrawData[] drawList, string data = "")
		{
			// if (string.IsNullOrEmpty(data)) data = Vendor.LuckyData;
			// if (string.IsNullOrEmpty(data))
			// {
			// 	Debug.LogError("=== 传入的时间戳为空 ===");
			// 	Close();
			// }
			//
			// btnPlay.interactable = true;
			// _onRolling = false;
			//
			// if (long.TryParse(data, out var stamp))
			// {
			// 	_luckDate = JollySDK.TimeStampToDate(stamp);
			// 	InitSpinItems(drawList);
			// 	StartCountdown();
			// }
			// else
			// {
			// 	Debug.LogError("=== 解析时间戳失败 ===");
			// 	Close();
			// }
			
		}
		
		
		/// <summary>
		/// 点击Play按钮
		/// </summary>
		private void OnClickPlay()
		{
			// if (_onRolling) return;
			//
			// StopCountdown(); // 先停止倒计时
			// _onRolling = true;
			//
			// Vendor.LuckyDraw(OnGetResult);
		}
		
		

		#region 抽奖管理

		/// <summary>
		/// 获得抽奖逻辑
		/// </summary>
		/// <param name="result"></param>
		private void OnGetResult(LuckydrawResult result)
		{
			if (result != null && result.Success)
			{
				_curResultData = result;
				// StartCoroutine(OnSpining(result.rewardUpId));
				StartRolling();
			}
			else
			{
				Debug.Log("=== 抽奖结果数失败 ===");
				Close();
			}

		}

		#endregion
		

	    #region 倒计时管理

	    public void StartCountdown()
	    {
		    if (_luckDate == null) return;
		    StopCountdown();
		    _countdown = StartCoroutine(OnCountdown(_luckDate));
	    }


	    public void StopCountdown()
	    {
		    if(_countdown != null) StopCoroutine(_countdown);
	    }


	    IEnumerator OnCountdown(DateTime date)
	    {
		    while (date > DateTime.Now)
		    {
			    var span = date - DateTime.Now;
			    txtTime.text = $"{span:mm\\:ss}";
			    yield return new WaitForSeconds(1);
		    }

		    btnPlay.interactable = false;
		    
		    yield return new WaitForSeconds(1);
		    
		    Close();
	    }

	    #endregion


	    #region 抽奖动画

		/// <summary>
		/// 开始抽奖动画
		/// </summary>
	    private void StartRolling()
	    {
		    _onRolling = true;
		    float angle = 360 * 5 + 45f * (_curResultData.rewardUpId - 1);
		    Debug.Log($"=== Spin Angle: {angle}");
		    float time = 2f;
		    board.DOLocalRotate(new Vector3(0, 0, angle), time, RotateMode.FastBeyond360)
			    .SetEase(Ease.OutQuad)
			    .OnComplete(() =>
			    {
				    Delay(0.5f, ShowLuckyResult);
			    });
	    }

	    private void ShowLuckyResult()
	    {
		    var popup = UI.OpenPopup<PopupLuckyResult>(G.Address.PopupLuckyResult);
		    popup.InitWithData(_curResultData.rewardValue);
			Close();
	    }


	    #endregion






    }


}
