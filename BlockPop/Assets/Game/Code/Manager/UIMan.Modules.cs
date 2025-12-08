namespace Game
{
    public partial class UIMan
    {
        // 显示抽奖页面
        public PopupLuckyDraw OpenLuckyDraw(LuckydrawData[] drawList, string data = "")
        {
            var popup = UI.OpenPopup<PopupLuckyDraw>(G.Address.PopupLuckyDraw);
            popup.InitWithData(drawList, data);
            return popup;
        }
        
    }
}