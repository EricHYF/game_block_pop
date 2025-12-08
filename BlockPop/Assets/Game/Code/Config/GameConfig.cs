using GKIT;
using UnityEngine;

namespace Game
{
    
    [CreateAssetMenu(fileName = "GameConfig", menuName ="Config/Create GameConfig" )]
    public class GameConfig : ConfigrationSO<GameConfig>
    {
       
        

        [Header("弹幕内容")] public string[] Barrages = new[]
        {
            "佳*慧-300",
            "李*-0.3",
            "郭*林-0.3",
            "林*佳-500",
            "刘*闻-300",
            "甲*-0.3",
            "*a-500",
            "莫*-0.3",
            "乃*期-0.3",
            "何*-300",
            "王**-0.3",
            "由*-200",
            "聂*-500",
        };


        protected override void InitDefault()
        {
            
        }
    }
}