using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class UIEntryFucard: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected Button btnEntry;
		protected Text txtBonus;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			btnEntry = FindUI<Button>("btn_entry");
			txtBonus = FindUI<Text>("txt_bonus");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}
		
		#region 属性

		

		#endregion

		#region 初始化

		protected override void Init()
		{
			base.Init();
			AddButton(btnEntry, OnClickEntry);

			// if (!Vendor.IsFucardOpen)
			// {
				Active = false;
				return;
			// }

			// txtBonus.text = $"{Vendor.FucardAmount}元";
			// AddEvent(Events.OnPopupMoneyClosed, OnPopupMoneyClosed);
		}

		private void OnClickEntry()
		{
			// Vendor.OpenFucard();
		}

		private void OnPopupMoneyClosed(object data)
		{
			// Vendor.ShowFucardView();
		}


		#endregion
		
		#region 活动相关调用
		
		
		
		
		

		#endregion
		
		
    }
}
