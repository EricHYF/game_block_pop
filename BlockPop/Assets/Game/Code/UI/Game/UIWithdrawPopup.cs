using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class UIWithdrawPopup: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected Text txtInfo;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			txtInfo = FindUI<Text>("txt_info");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}


		
		protected override void Init()
		{
			base.Init();
			
			
		}

		#region 信息提示
		
		/// <summary>
		/// 显示提示信息
		/// </summary>
		/// <param name="info"></param>
		public void ShowInfo(GUIInfoData info)
		{
			int status = info.phase;
			// if (!Vendor.IsBusinessOpen) status = -1;
			Active = true;
			// if (Vendor.IsUserReachLevelLimit)
			// {
			// 	Active = false;
			// 	return;
			// }
			switch (status)
			{
				case 1:
					txtInfo.text = $"再赚<color=#DB8C4D>{info.firstValue}</color>元\n可提现<color=#FB7E3D>{info.secondValue}</color>元";
					break;
				case 2:
					txtInfo.text = $"今日再过<color=#DB8C4D>{info.firstValue}</color>关\n即可<color=#FB7E3D>提现</color>";
					break;
				case 3:
					txtInfo.text = $"再过<color=#DB8C4D>{info.firstValue}</color>关\n即可升级<color=#FB7E3D>{int.Parse(info.secondValue)+1}</color>级";
					break;
				case 4:
					txtInfo.text = $"当日已进入防沉迷\n明日奖励翻倍";
					break;
				default:
					txtInfo.text = "";
					Active = false;
					break;
			}

		}
		

		#endregion

    }
}
