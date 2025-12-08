using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TileData
    {
        public int id = 0;
        public string name;
        public int x;
        public int y;
        public Tile view;

        public int OrderV => y * 10 + x;
        public int OrderH => y + x * 10;
        
        public TileData up;
        public TileData down;
        public TileData left;
        public TileData right;
        
        public int moveX;
        public int moveY;
        public Vector2 MovePos => new Vector2(x + moveX, y + moveY);

        public bool IsDisabled => id == Const.TILE_ID_EMPTY;


        public static void Exchange(TileData a, TileData b)
        {
            var id1 = a.id;
            var id2 = b.id;
            a.SetID(id2);
            b.SetID(id1);
        }
        
        /// <summary>
        /// 设置为隐藏状态
        /// </summary>
        public void SetDisable() => SetID(Const.TILE_ID_EMPTY);

        public void SetID(int id)
        {
            this.id = id;
            if (null != view)
            {
                view.Active = true;
                // Debug.Log($"{view.name} -> {id-1}");
                view.SetIcon(id-1);
                view.UpdateInfo();
            }
        }

        public List<TileData> CheckAround(int tid = -1)
        {
            if (tid == -1) tid = id;
            List<TileData> list = new List<TileData>(4);
            
            if (null != left && left.id == tid)
            {
                list.Add(left);
            }
            if (null != up  && up.id == tid)
            {
                list.Add(up);   
            }
            if (null != right && right.id == tid)
            {
                list.Add(right);
            }

            if (null != down && down.id == tid)
            {
                list.Add(down);
            }
            

            


            return list;
        }

        public void ClearMoveData()
        {
            moveX = moveY = 0;
        }
        
    }
}