using System;
using System.ComponentModel.Design.Serialization;
using GKIT;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LogoSwitchEditor: EditorWindow
    {
        
        private static LogoSwitchEditor _instance;

        public static LogoSwitchEditor Instance
        {
            get
            {
                if (_instance == null) _instance = CreateInstance<LogoSwitchEditor>();
                return _instance;
            }
        }

        private LogoSwitchConfig _config;
        private int _selectIndex = 0;
        private string[] _setNames;
        private Sprite _logoSprite;
        private string _selectLabel;

        #region 工具接口

        [MenuItem("Tools/LOGO切换器", false, 10)]
        public static void Open()
        {
            Instance.Show();
        }

        #endregion
        
        

        #region  初始化

        private void OnEnable()
        {
            _config = LogoSwitchConfig.EditorLoading();

            int count = _config.settings.Length;

            _setNames = new string[count];
            for (int i = 0; i < count; i++)
            {
                _setNames[i] = _config.settings[i].name;
            }

            _selectLabel = "请选择要切换的LOGO";
        }

        #endregion

        #region GUI

        
        void OnGUI()
        {

            GUILayout.Space(5);

            var s = GKIT.GUIHelper.BoxStyle();
            s.fixedHeight = 60;
            GUILayout.Label("LOGO切换器", s);

            GUILayout.Space(5);
            GKIT.GUIHelper.ColorBox(Color.yellow, () =>
            {
                if (GUILayout.Button("更改设置"))
                {
                    Selection.activeObject = _config;
                }
            });

            GUILayout.Space(10);
            
            GUILayout.Label("--- 请选择要切换的图标 ---");


            int index = EditorGUILayout.Popup(_selectLabel, _selectIndex, _setNames);
            if(index != _selectIndex)
            {
                _selectIndex = index;
                // _logoTexture = AssetPreview.GetAssetPreview(_config.settings[_selectIndex].loadingLogo);
                _logoSprite = _config.settings[_selectIndex].loadingLogo;

                _selectLabel = $"设置LOGO [{_selectIndex}]:";
            }
            // GUILayout.Label(_logoTexture);
            if (_logoSprite != null)
            {
                GUILayout.Space(4);
                // 显示
                EditorGUILayout.ObjectField(_config.settings[_selectIndex].loadingLogo, 
                    typeof(Sprite), true, GUILayout.Height(140) );
                
                GUILayout.Space(4);
                GKIT.GUIHelper.ColorBox(Color.green, () =>
                {
                    if (GUILayout.Button("替换组件", GUILayout.Height(60)))
                    {
                        Debug.Log($"--> 替换Logo: <color=orange>{_config.settings[_selectIndex].name}</color>");
                        ApplyChanges();
                    }
                });
            }

   
            
            
        }
        


        #endregion
        
        
        #region 应用更改
        
        /// <summary>
        /// 应用更改
        /// </summary>
        private void ApplyChanges()
        {
            var settings = _config.settings[_selectIndex];
            
            // 替换Splash屏贴图
            PlayerSettings.SplashScreen.background = settings.splash;
            PlayerSettings.SplashScreen.blurBackgroundImage = false;
            
            // 替换组件内容
            GameObject prefab = PrefabUtility.LoadPrefabContents(_config.assetPath);
            if (prefab != null)
            {
                GameObject go = PrefabUtility.LoadPrefabContents(_config.assetPath);
                if (go == null)
                {
                    Debug.LogError($"--- Canot find GO:{_config.assetPath}");
                    return;
                }

                var tran = go.transform.Find(_config.targetPath);
                if (tran != null)
                {
                    Debug.Log($"--> 替换Prefab: <color=#88ff00>{tran.name}</color>\n{_config.targetPath}");
                    var img = tran.GetComponent<Image>();
                    img.sprite = settings.loadingLogo;
                    img.SetNativeSize();
                    PrefabUtility.SaveAsPrefabAsset(go, _config.assetPath);
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(_config.assetPath);
                }
                else
                {
                    Debug.LogError($"--- Canot find Trans: {_config.targetPath}");    
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }


        #endregion
        
        
    }
}