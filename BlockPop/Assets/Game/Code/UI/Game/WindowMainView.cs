using System;
using System.Collections;
using TMPro;
using GKIT.UI;
using GKIT;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    
    public class WindowMainView: WindowBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected RectTransform top;
		protected RectTransform tileNode;
		protected RectTransform tilePrefab;
		protected ScoreItem scorePrefab;
		protected TextMeshProUGUI txtScore;
		protected RectTransform scoreNode;
		protected TextMeshProUGUI txtLevel;
		protected TextMeshProUGUI txtGoal;
		protected TileSpritesLib tileSprits;
		protected RectTransform levelInfo;
		protected RectTransform mergeInfo;
		protected RectTransform withdrawPopup;
		protected RectTransform luckydrawProgress;
		protected SkeletonGraphic txRedbagfly;
		protected RectTransform userMoneyInfo;
		protected Button btnSettings;
		protected RectTransform maskNode;
		protected RawImage userIcon;
		protected RectTransform bulletPopScreen;
		protected RectTransform activityFucard;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			top = FindUI<RectTransform>("top");
			tileNode = FindUI<RectTransform>("tile_node");
			tilePrefab = FindUI<RectTransform>("tile_prefab");
			scorePrefab = FindUI<ScoreItem>("score_prefab");
			txtScore = FindUI<TextMeshProUGUI>("txt_score");
			scoreNode = FindUI<RectTransform>("score_node");
			txtLevel = FindUI<TextMeshProUGUI>("txt_level");
			txtGoal = FindUI<TextMeshProUGUI>("txt_goal");
			tileSprits = FindUI<TileSpritesLib>("tile_sprits");
			levelInfo = FindUI<RectTransform>("level_info");
			mergeInfo = FindUI<RectTransform>("merge_info");
			withdrawPopup = FindUI<RectTransform>("withdraw_popup");
			luckydrawProgress = FindUI<RectTransform>("luckydraw_progress");
			txRedbagfly = FindUI<SkeletonGraphic>("tx_redbagfly");
			userMoneyInfo = FindUI<RectTransform>("user_money_info");
			btnSettings = FindUI<Button>("btn_settings");
			maskNode = FindUI<RectTransform>("mask_node");
			userIcon = FindUI<RawImage>("user_icon");
			bulletPopScreen = FindUI<RectTransform>("bullet_pop_screen");
			activityFucard = FindUI<RectTransform>("activity_fucard");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}

		public RectTransform TileRoot => tileNode;
		public GameObject TilePrefab => tilePrefab.gameObject;
		public GameObject ScorePrefab => scorePrefab.gameObject;
		public RectTransform ScoreNode => scoreNode;
		public TileSpritesLib TileSprites => tileSprits;


		private UILevelInfo _levelInfo;
		private UIMergeInfo _mergeInfo;
		private UIWithdrawPopup _withdrawBubble;
		private UILuckyDrawProgress _luckyDrawProgress;
		private UIUserMoneyInfo _userMoneyInfo;
		private UIBulletPopScreen _bulletScreen;
		private UIEntryFucard _fucardEntry;

		private int _userIconState;
		

		#region 初始化
		
		/// <summary>
		/// 初始化
		/// </summary>
		protected override void Init()
		{
			base.Init();
			
			//-------------- 屏幕适配 -----------------
			if (UI.Fitter.IsLongScreen)
			{
				// _background_top.anchoredPosition = new Vector2(0, -ScreenFitter.SAFE_TOP_HEIGHT);
				top.anchoredPosition = new Vector2(0, -ScreenFitter.SAFE_TOP_HEIGHT);
			}

			_userIconState = 0;
			
			_levelInfo = AddSubUI<UILevelInfo>(levelInfo);
			_mergeInfo = AddSubUI<UIMergeInfo>(mergeInfo);
			_withdrawBubble = AddSubUI<UIWithdrawPopup>(withdrawPopup);
			_luckyDrawProgress = AddSubUI<UILuckyDrawProgress>(luckydrawProgress);
			_userMoneyInfo = AddSubUI<UIUserMoneyInfo>(userMoneyInfo);
			_bulletScreen = AddSubUI<UIBulletPopScreen>(bulletPopScreen);
			_fucardEntry = AddSubUI<UIEntryFucard>(activityFucard);
			
			tileSprits.Init();

			txRedbagfly.transform.position = _userMoneyInfo.IconPos;  // 特效和红包图表对齐
			
			_withdrawBubble.Active = false;
			HideRedPacketFlying();

			// if (Vendor.IsBusinessOpen)
			// {
			// 	UpdateLocalLuckyDraw();
			// 	StartHeartBeat();	
			// 	DisplayLocalLuckyDraw(Vendor.IsLuckyDrawEnable);
			// }
			// else
			{
				// 关闭商业化后的所有表现
				_withdrawBubble.Active = false;
				_luckyDrawProgress.Active = false;
				_userMoneyInfo.Active = false;
				DisplayLocalLuckyDraw(false);
			}
			
			
			AddButton(btnSettings, OnOpenSetting);
			Sound.PlayMusic(Sound.GetBGMKey("bgm"));
		}

		private void OnOpenSetting()
		{
			UI.OpenPopup<PopupSetting>(G.Address.PopupSetting);
		}

		private void OnOpenTask()
		{
			Debug.Log($"------ Open Task ------");
		}

		#endregion

		#region 棋盘管理

		/// <summary>
		/// 初始化盘面
		/// </summary>
		private void InitBoard()
		{
			
		}
		

		#endregion
		
		#region UI 控制

		public void SetScore(int score)
		{
			txtScore.text = $"{score}";
		}

		public void SetGoalScore(int goal)
		{
			txtGoal.text = $"{goal}";
		}

		public void SetLevel(int level)
		{
			txtLevel.text = $"{level}";
		}


		/// <summary>
		/// 进入关卡
		/// </summary>
		/// <param name="level"></param>
		/// <param name="goal"></param>
		/// <param name="callback"></param>
		public void EnterLevel(int level, int goal, Action callback)
		{
			SetLevel(level);
			SetGoalScore(goal);
			
			_levelInfo.OpenWithData(level, goal, callback);

			// if (Vendor.IsShowLevelInfo)
			// {
			// 	 _levelInfo.OpenWithData(level, goal, callback);
			// }
			// else
			// {
			// 	 callback?.Invoke();
			// }
		}
		
		public void ShowMergeInfo()
		{
			_mergeInfo.Show();
		}


		public void ShowRedPacketFlying()
		{
			txRedbagfly.gameObject.SetActive(true);
			txRedbagfly.PlayAnim("start", () =>
			{
				txRedbagfly.gameObject.SetActive(false);
				_userMoneyInfo.StopMoneyPop();
			});
			
			_userMoneyInfo.StartMoneyPop(0.8f);
		}

		public void HideRedPacketFlying()
		{
			txRedbagfly.gameObject.SetActive(false);
		}
		

		#endregion
		
		#region 心跳事件

		private bool _hasWithdraw;
			
		/// <summary>
		/// 心跳事件
		/// </summary>
		private void StartHeartBeat()
		{
			// if (!Vendor.IsBusinessOpen) return;

			return;
			_hasWithdraw = false;
			float heartBeatInterval = 2f;

			// StartCoroutine(UpdateHeartBeat(heartBeatInterval));
		}

		// IEnumerator UpdateHeartBeat(float hearBeatInterval)
		// {
		// 	while (true)
		// 	{
		// 		yield return new WaitForSeconds(hearBeatInterval);
		// 		_hasWithdraw = Vendor.HasWithdraw();
		// 		
		// 		// 提现气泡文本刷新
		// 		_withdrawBubble.ShowInfo(Vendor.GetGuideTextData());
		// 		
		// 		// 更新抽奖进度
		// 		UpdateLocalLuckyDraw();
		// 		
		// 		// 更新用户头像
		// 		UpdateUserIcon();
		// 	}
		// }

		/// <summary>
		/// 更新用户Icon
		/// </summary>
		// private void UpdateUserIcon()
		// {
		// 	switch (_userIconState)
		// 	{
		// 		case 0:
		// 			if (Vendor.IsWXBind)
		// 			{
		// 				_userIconState = 2;  // 加载中
		// 				Vendor.LoadUserIcon(t2d =>
		// 				{
		// 					if (t2d != null)
		// 					{
		// 						User.UserIcon = t2d;
		// 						_userIconState = 1;
		// 						SetUserIcon(t2d);
		// 					}
		// 				});
		// 			}
		// 			break;
		// 		case 1:
		// 			// 有头像时解绑, 则不再显示
		// 			if (!Vendor.IsWXBind)
		// 			{
		// 				User.UserIcon = null;
		// 				ClearUserIcon();
		// 				_userIconState = 0;
		// 			}
		// 			break;
		// 	}
		// }



		#endregion
		
		#region 本地抽奖提示

		/// <summary>
		/// 更新游戏下方的抽奖进度
		/// </summary>
		/// <param name="immediately"></param>
		public void UpdateLocalLuckyDraw()
		{
			// if (Vendor.IsLuckyDrawEnable)
			// {
			// 	DisplayLocalLuckyDraw(true);
			// }
			// else
			// {
			// 	DisplayLocalLuckyDraw(false);
			// 	return;
			// }
			//
			// _luckyDrawProgress.UpdateInfo();
		}

		/// <summary>
		/// 是否显示本地抽奖条
		/// </summary>
		/// <param name="flag"></param>
		private void DisplayLocalLuckyDraw(bool flag)
		{
			if (flag != _luckyDrawProgress.Active)
			{
				_luckyDrawProgress.Active = flag;
			}
		}
		
		
		#endregion
		
		#region 弹幕控制

		public void InitBulletScreen()
		{
			// if (GameController.Instance.BulletScreenSwitch)
			// {
			// 	ShowBulletScreen();
			// }
			// else
			{
				HideBulletScreen();
			}
		}
		
		/// <summary>
		/// 显示弹幕
		/// </summary>
		public void ShowBulletScreen()
		{
			_bulletScreen.Show();
		}
		
		/// <summary>
		/// 隐藏弹幕
		/// </summary>
		public void HideBulletScreen()
		{
			_bulletScreen.Hide();
		}


		public void OnBulletScreenSwitch(bool value)
		{
			if (value)
			{
				ShowBulletScreen();
			}
			else
			{
				HideBulletScreen();
			}
		}
		#endregion
		
		#region 头像设置

		/// <summary>
		/// 设置用户头像
		/// </summary>
		/// <param name="t2d"></param>
		private void SetUserIcon(Texture2D t2d)
		{
			maskNode.gameObject.SetActive(true);
			userIcon.texture = t2d;
		}
		
		/// <summary>
		/// 隐藏用户头像
		/// </summary>
		private void ClearUserIcon()
		{
			maskNode.gameObject.SetActive(false);
			userIcon.texture = null;
		}
		


		#endregion
		
    }
}
