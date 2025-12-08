using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LuckyDrawSpinItem: MonoBehaviour
    {

        [SerializeField] private Text _txtTitle;
        [SerializeField] private GameObject _icon1;
        [SerializeField] private GameObject _icon2;

        private int _index;

        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        public void Init(LuckydrawData data, int index)
        {
            _index = index;
            name = $"spin_item_{(index + 1)}";
            _icon1.SetActive(false);
            _icon2.SetActive(false);

            float offAng = -45f;

            transform.eulerAngles = new Vector3(0, 0, offAng * index); // 自转角度

            if (data.rewardType == 1)
            {
                // 1是现金红包
                _icon1.SetActive(true);
                _txtTitle.text = $"现金礼包";
            }
            else
            {
                // 其他事提现机会
                _icon2.SetActive(true);
                _txtTitle.text = $"提现{data.rewardValue}元";
            }

        }
        
    }
}