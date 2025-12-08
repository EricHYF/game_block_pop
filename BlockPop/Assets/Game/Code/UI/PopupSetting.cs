using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;

namespace Game
{
    public class PopupSetting: PopupBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform mask;
		protected RawImage head;
		protected Text txtUserName;
		protected Text txtUserLevel;
		protected UISimpleToggle toggleSound;
		protected UISimpleToggle toggleBullet;
		protected Button btnPrivacy;
		protected Button btnServices;
		protected Button btnLogout;
		protected Button btnClose;
		protected Button btnFeed;
		protected Button btnQuit;
		protected Button btnLogin;
		protected RectTransform groupFeedback;
		protected RectTransform groupBullet;
		protected RectTransform groupLogout;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			mask = FindUI<RectTransform>("mask");
			head = FindUI<RawImage>("head");
			txtUserName = FindUI<Text>("txt_user_name");
			txtUserLevel = FindUI<Text>("txt_user_level");
			toggleSound = FindUI<UISimpleToggle>("toggle_sound");
			toggleBullet = FindUI<UISimpleToggle>("toggle_bullet");
			btnPrivacy = FindUI<Button>("btn_privacy");
			btnServices = FindUI<Button>("btn_services");
			btnLogout = FindUI<Button>("btn_logout");
			btnClose = FindUI<Button>("btn_close");
			btnFeed = FindUI<Button>("btn_feed");
			btnQuit = FindUI<Button>("btn_quit");
			btnLogin = FindUI<Button>("btn_login");
			groupFeedback = FindUI<RectTransform>("group_feedback");
			groupBullet = FindUI<RectTransform>("group_bullet");
			groupLogout = FindUI<RectTransform>("group_logout");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}

		public const string USER_DEFAULT_NAME = "恭喜发财";
		private bool _isLogin;

		#region 初始化

		protected override void Init()
		{
			base.Init();

			toggleBullet.Value = GameController.Instance.BulletScreenSwitch;
			toggleBullet.OnValueChanged = OnBulletScreenChanged;

			toggleSound.Value = Sound.SoundEnabled;
			toggleSound.OnValueChanged = OnSoundChanged;
			
			AddButton(btnPrivacy, OnOpenPrivacy);
			AddButton(btnServices, OnOpenServices);
			AddButton(btnLogout, OnLogoutAccount);
			AddButton(btnQuit, OnQuitAccount);
			AddButton(btnFeed, OnOpenFeedback);
			AddButton(btnClose, Close);
            AddButton(btnLogin, OnLoginAccount);

#if UNITY_IOS
			// ------------- iOS不显示【意见反馈】-------------------
			groupFeedback.gameObject.SetActive(false);  
			groupBullet.gameObject.SetActive(false);
			groupLogout.gameObject.SetActive(false);
#endif
        
			UpdateUserInfo();
		}

		#endregion

		#region 用户信息更新

		private void UpdateUserInfo()
		{
			txtUserName.text = USER_DEFAULT_NAME;
			mask.gameObject.SetActive(false);
			btnLogin.gameObject.SetActive(false);
			// _isLogin = Vendor.IsWXBind;
			// if (_isLogin)
			// {
			// 	var info = Vendor.GetWxUserInfo();
			// 	if (!string.IsNullOrEmpty(info.wx_uname)) txtUserName.text = info.wx_uname;
			//
			// 	if (User.UserIcon != null)
			// 	{
			// 		// 显示头像
			// 		mask.gameObject.SetActive(true);
			// 		head.texture = User.UserIcon;
			// 	}
			// }
			// else
			{
				btnLogin.gameObject.SetActive(true);
				btnQuit.gameObject.SetActive(false);
			}
		}
		

		#endregion
		
		#region 音效控制


		private void OnSoundChanged(bool value)
		{
			Sound.SoundEnabled = value;
			Sound.MusicEnabled = value;
		}
		

		#endregion

		#region 弹幕控制

		/// <summary>
		/// 弹幕控制
		/// </summary>
		/// <param name="value"></param>
		private void OnBulletScreenChanged(bool value)
		{
			GameController.Instance.BulletScreenSwitch = value;
		}
		

		#endregion
		
		#region 链接跳转


		private void OnOpenPrivacy()
		{
			// Vendor.OpenPrivacyURL();
		}

		private void OnOpenServices()
		{
			// Vendor.OpenTermsURL();
		}

		private void OnOpenFeedback()
		{
			// Vendor.OpenFeedbackURL();
		}
		

		#endregion

		#region 账户管理
		
		/// <summary>
		/// 登录账户
		/// </summary>
		private void OnLoginAccount()
		{
			// Vendor.WXBind((success, json) =>
			// {
			// 	UpdateUserInfo();
			// });

			if (!_isLogin)
			{
				UI.Toast("请到提现页点击提现按钮登录");
			}
		}
		
		
		
		
		/// <summary>
		/// 登出账户
		/// </summary>
		private void OnLogoutAccount()
		{
			if (_isLogin)
			{
				UI.Toast("账号已成功注销，将在7日内审核");
				UnbindWX();
			}
			else
			{
				UI.Toast("您还未登录账号");
			}

		}


		private void OnQuitAccount()
		{
			if (_isLogin)
			{
				UI.Toast("您已成功退出登录");
				UnbindWX();
			}
		}

		private void UnbindWX()
		{
			// Vendor.WXUnbind();
			mask.gameObject.SetActive(false);
			head.texture = null;
			txtUserName.text = USER_DEFAULT_NAME;
			
			UpdateUserInfo();
		}
		

		#endregion
    }
}
