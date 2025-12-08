namespace Game
{

    public enum Events
    {
        OpenWindow,
        OpenPopup,
        OnPopupMoneyClosed,
        
        OnClickTile,
        OnShowMergeInfo,

    }

    /// <summary>
    /// 全局类
    /// </summary>
    public static class G
    {
        /// <summary>
        /// 各组件地址
        /// </summary>
        public partial class Address
        {
        
            // Game
            public static string WindowMainView = "game/window_main_view";      
            
            // Popup
            public static readonly string PopupToast = "ui/popup_toast/popup_toast";
            public static readonly string PopupMoney = "ui/popup_money/popup_money";
            public static readonly string PopupGuideMoney = "ui/popup_guide_money/popup_guide_money";
            public static readonly string PopupCombinMoney = "ui/popup_combo_money/popup_combine_money";
            public static readonly string PopupHuanChongMoney = "ui/popup_combine_money/popup_huanchong_money";
            public static readonly string PopupHuanChongMoneyFail = "ui/popup_combine_money/popup_huanchong_money_fail";
            public static readonly string PopupFirstWithdrawClaim = "ui/popup_first_withdraw_claim/popup_first_withdraw_claim";
            public static readonly string PopupSetting = "ui/popup_settings/popup_setting";
            
            // 本地抽奖组件
            public static readonly string PopupLuckyDraw = "ui/popup_luckydraw/popup_luckydraw";
            public static readonly string PopupLuckyResult = "ui/popup_luckydraw/popup_luckydraw_result";
        }
        
        
    }
    

}