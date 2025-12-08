using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    
    /// <summary>
    /// 微信头像库
    /// </summary>
    public class UIBulletHead: MonoBehaviour
    {
        [SerializeField] private Image _headIcon;
        [SerializeField] private Sprite[] _headers;
        
        private static int _headerIndex = 0;
        private static bool _hasShuffled = false;

        private void Awake()
        {
            // _headIcon.sprite = null;
            ShuffleHeaders();
        }
        
        /// <summary>
        /// 头像乱序
        /// </summary>
        private void ShuffleHeaders()
        {
            if (_hasShuffled) return;
            _hasShuffled = true;
            for (int i = 0; i < _headers.Length; i++)
            {
                if (i +1 < _headers.Length - 1)
                {
                    var idx = Random.Range(i + 1, _headers.Length);
                    (_headers[i], _headers[idx]) = (_headers[idx], _headers[i]);
                }
            }
        }
        

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (_headerIndex >= _headers.Length) _headerIndex = 0;
            _headIcon.sprite = _headers[_headerIndex];
            // Debug.Log($"_headerIndex: {_headerIndex}");
            _headerIndex++;
        }
        
    }
}