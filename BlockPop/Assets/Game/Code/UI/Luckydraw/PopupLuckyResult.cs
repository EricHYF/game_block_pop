using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class PopupLuckyResult: PopupBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected Button btnGo;
		protected Button btnClose;
		protected Text txtNum;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			btnGo = FindUI<Button>("btn_go");
			btnClose = FindUI<Button>("btn_close");
			txtNum = FindUI<Text>("txt_num");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}


		protected override void Init()
		{
			base.Init();
			AddButton(btnGo, Close);
			AddButton(btnClose, Close);
		}

		public void InitWithData(string value)
		{
			txtNum.text = $"+{value}<size=100>元</size>";
		}


    }
}
