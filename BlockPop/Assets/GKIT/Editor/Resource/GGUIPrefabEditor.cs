using System;
using System.IO;
using GKIT.UI;
using SRF;
using UnityEditor;
using UnityEngine;

namespace GKIT.Editor
{
    /// <summary>
    /// Prefab编辑器
    /// </summary>
    public class GGUIPrefabEditor : EditorWindow
    {
        public const string K_TITLE_NAME = "UI管理";
        private const string K_DEFAULT_PREFAB_NAME = "new_view";


        private static GGUIPrefabEditor _instance;

        public static GGUIPrefabEditor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GetWindow<GGUIPrefabEditor>();
                    _instance.Init();
                }
                return _instance;
            }
        }


        [MenuItem(GResBuilder.K_EDITOR_MENU_ROOT + "/[" + K_TITLE_NAME + "]/创建基础UI")]
        static void BuildNewPrefab()
        {
            Instance.Show();
        }




        private string _rootDir;
        private string _prefabName;




        #region 初始化


        public void Init()
        {
            if (_instance != null) return;
            _instance = this;
            
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("UIPrefab 编辑器");
            _rootDir = GResBuilder.ResourcesPath;
            _prefabName = K_DEFAULT_PREFAB_NAME;
        }

        #endregion

        #region GUI

        void OnGUI()
        {
            GUILayout.Space(5);
            GUI_PrefabInfo();
        }

        private void GUI_PrefabInfo()
        {
            
      
            GUILayout.Label("存放路径");
            GUILayout.BeginHorizontal();
            _rootDir = GUILayout.TextField(_rootDir, GUILayout.Width(300));
            this.GButton("选择", () =>
            {
                string path = EditorUtility.OpenFolderPanel("选择存放路径", _rootDir, "");
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    _rootDir = path;
                }
            }, width: 60);
            GUILayout.EndHorizontal();
   
            
            GUILayout.Space(2);
            
   
            GUILayout.Label("预置名称");
            _prefabName = GUILayout.TextField(_prefabName);
            
            
            this.GButtonGreen("创建", () =>
            {
                CreateUIPrefab();
            }, height: 60);
        
        }

        #endregion

        #region 创建Prefab
        
        /// <summary>
        /// 创建UI预制体
        /// </summary>
        private void CreateUIPrefab()
        {
            GameObject go = new GameObject(_prefabName);
            go.AddComponent<CanvasRenderer>();
            CreateExtendsRT(go);
            var root = go.transform.CreateChild("root");
            CreateExtendsRT(root);

            var binder = go.AddComponent<UIBinder>();
            
            string path = _rootDir.Substring(_rootDir.IndexOf("Assets")) + "/" + _prefabName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(go, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            DestroyImmediate(go);
                
        }

        private RectTransform CreateExtendsRT(GameObject go)
        {
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt == null) rt = go.gameObject.AddComponent<RectTransform>();
            rt.anchorMax = Vector2.one;
            rt.anchorMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;

            rt.localScale = Vector3.one;
            return rt;
        }

        #endregion
        
        
        
    }
}