using UnityEngine;

namespace GKIT
{
    public class GDebug
    {

        public static bool IsDebug
        {
            get
            {
    #if UNITY_EDITOR
                return true;
    #endif
                return Debug.isDebugBuild;
            }
        }
        
    }


    public class GLog
    {

        public const string COLOR_GREEN = "#88ff00";
        public const string COLOR_ORANGE = "orange";
        public const string COLOR_RED = "red";
        public const string COLOR_WHITE= "white";
        
        
        public static void M(object content, string colorCode = "")
        {
            if (!GDebug.IsDebug) return;
            string message = content.ToString();
            if (string.IsNullOrEmpty(colorCode)) colorCode = COLOR_WHITE;
#if UNITY_EDITOR
            message = $"<color={colorCode}>{message}</color>";
#endif
            Debug.Log(message);
        }
        
        
        public static void D(object content, string colorCode = "")
        {
            if (!GDebug.IsDebug) return;
            string message = content.ToString();
            if (string.IsNullOrEmpty(colorCode)) colorCode = COLOR_GREEN;
#if UNITY_EDITOR
            message = $"<color={colorCode}>{message}</color>";
#endif
            Debug.Log(message);
        }
        
        public static void W(object content, string colorCode = "")
        {
            if (!GDebug.IsDebug) return;
            string message = content.ToString();
            if (string.IsNullOrEmpty(colorCode)) colorCode = COLOR_ORANGE;
#if UNITY_EDITOR
            message = $"<color={colorCode}>{message}</color>";
#endif
            Debug.Log(message);
        }
        
        public static void E(object content, string colorCode = "")
        {
            if (!GDebug.IsDebug) return;
            string message = content.ToString();
            if (string.IsNullOrEmpty(colorCode)) colorCode = COLOR_RED;
#if UNITY_EDITOR
            message = $"<color={colorCode}>{message}</color>";
#endif
            Debug.Log(message);
        }
    }
}