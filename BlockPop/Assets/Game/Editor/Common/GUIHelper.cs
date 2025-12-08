using System;
using UnityEngine;

namespace GKIT
{
    public class GUIHelper
    {

        /// <summary>
        /// 颜色区域
        /// </summary>
        /// <param name="color"></param>
        /// <param name="content"></param>
        public static void ColorBox(Color color, Action content)
        {
            var c = GUI.color;
            GUI.color = color;
            content?.Invoke();
            GUI.color = c;
        }


        /// <summary>
        /// 通用Box区块样式
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static GUIStyle BoxStyle(float width = 360, float height = 26)
        {
            var s = new GUIStyle("box");
            s.fontSize = 20;
            s.alignment = TextAnchor.MiddleCenter;
            s.fontStyle = FontStyle.Bold;
            s.fixedWidth = width;
            s.fixedHeight = height;

            return s;
        }


    }
}