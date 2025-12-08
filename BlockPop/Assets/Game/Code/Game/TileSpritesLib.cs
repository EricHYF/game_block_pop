using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class TileSpritesLib:MonoBehaviour
    {
        [Header("网格块贴图")]
        [SerializeField] private Sprite[] _sprites;
        public Sprite GetTileSprite(int index)
        {
            if(index >=0 && index <_sprites.Length ) return _sprites[index];
            return null;
        }
        public int TileSpritesCount => _sprites.Length;

        private int _curIndex;
        private List<int> _orders;
        
        
        public void Init()
        {
            _curIndex = -1;
            _orders = new List<int>(_sprites.Length);
            int i = 0;
            while (i < _sprites.Length)
            {
                _orders.Add(i);
                i++;
            }
        }

        private void Shuffle()
        {
            int count = _orders.Count;
            for (int i = 0; i < count; i++)
            {
                if(count-i > 2)
                {
                    int idx = Random.Range(i, _orders.Count);
                    (_orders[i], _orders[idx]) = (_orders[idx], _orders[i]);
                }
            }
        }

        private void Reset()
        {
            _curIndex = 0;
            Shuffle();
        }
        
        /// <summary>
        /// 获取下一个贴图
        /// </summary>
        /// <returns></returns>
        public Sprite GetNextSprite()
        {
            _curIndex++;
            if (_curIndex < 0 || _curIndex >= _sprites.Length)
            {
                _curIndex = 0;
                Shuffle();
            }
            return _sprites[_curIndex];
        }

        /// <summary>
        /// 获取贴图组
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Sprite[] GetSprites(int count = 2)
        {
            List<Sprite> list = new List<Sprite>(count);
            while(list.Count < count)
            {
                var sp = GetNextSprite();
                if(list.IndexOf(sp) == -1) list.Add(sp); 
            }
            return list.ToArray();
        }
        
    }
}