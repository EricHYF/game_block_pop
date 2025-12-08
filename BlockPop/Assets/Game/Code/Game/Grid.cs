using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using DG.Tweening;
using GKIT.Event;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public class Grid
    {

        private const int RESULT_GAMING = 0;
        private const int RESULT_FINISH = 1;
        private const int RESULT_NOMOVE = 2;

        private SoundMan Sound => SoundMan.Instance;
        private Dictionary<string, TileData> _tiles;
        private GameObject _tilePrefab;
        private GameObject _scorePrefab;
        private RectTransform _tileRoot;
        private Vector2 _size;
        
        private List<TileData> _findList;
        private List<TileData> _moveList;
        private List<TileData> _activeList;
        private List<TileData> _hintList;
        
        
        private UIMan UI => UIMan.Instance;
        private Vector3 _scoreToPos;
        private bool _enableClick;
        
        private List<ScoreItem> _scoreItems;
        private Sprite[] _tileIcons;
        public Action<int> OnAddScore;
        public Action OnFinishLevel;
        
        #region 初始化

        
        public void Init(RectTransform root, GameObject tile, GameObject score, RectTransform scoreNode)
        {
            _tileRoot = root;
            _scoreToPos = scoreNode.position;
            _tilePrefab = tile;
            _tilePrefab.gameObject.SetActive(false);
            _scorePrefab = score;
            _scorePrefab.gameObject.SetActive(false);
            
            _size = new Vector2();
            _size.x = Const.TILE_WITH * Const.GRID_W;
            _size.y = Const.TILE_HEIGHT * Const.GRID_H;
            var offset = new Vector2(-_size.x + Const.TILE_WITH, _size.y - Const.TILE_HEIGHT) * 0.5f;

            var totalCount = Const.GRID_W * Const.GRID_H;
            _activeList = new List<TileData>(totalCount);

            _tiles = new Dictionary<string, TileData>(totalCount);
            for (int i = 0; i < Const.GRID_W; i++)
            {
                for (int j = 0; j < Const.GRID_H; j++)
                {
                    TileData data = new TileData();
                    data.id = 0;
                    data.x = j;
                    data.y = i;
                    var key = GetTileKey(data.x, data.y);
                    data.name = key;
                    
                    // 方向绑定
                    if (data.x > 0)
                    {
                        var t = Find(data.x - 1, data.y);
                        if (t != null)
                        {
                            t.right = data;
                            data.left = t;
                        }
                    }
                    
                    // 方向绑定
                    if (data.y > 0)
                    {
                        var t = Find(data.x, data.y - 1);
                        if(t != null)
                        {
                            t.down = data;
                            data.up = t;
                        }
                    }

                    CreateTile(data, offset);
                    
                    // 列表
                    _tiles[key] = data;
                    data.view.Active = false;
                }
            }
            
            GEvent.Bind(Events.OnClickTile.ToString(), OnClickTile );
            _findList = new List<TileData>(50);
            _moveList = new List<TileData>(50);
            InitScoreItems();
        }
        
        
        
        
        /// <summary>
        /// 生成砖块名称
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string GetTileKey(int x, int y)
        {
            return $"t_{x}_{y}";
        }

        public TileData Find(int x, int y)
        {
            var name = GetTileKey(x, y);
            return Find(name);
        }

        public TileData Find(string name)
        {
            if (_tiles.TryGetValue(name, out var tile))
            {
                return tile;
            }
            
            return null;
        }

        
        
        /// <summary>
        /// 创建Tile块
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Tile CreateTile(TileData data, Vector2 offset)
        {
            var go = Object.Instantiate(_tilePrefab, _tileRoot);
            var tile = go.GetComponent<Tile>();
            if(null != tile)
            {
                data.view = tile;
                float x = data.x * Const.TILE_WITH + offset.x;
                float y = -data.y * Const.TILE_HEIGHT + offset.y;
                tile.SetPos(x, y);
                tile.name = data.name;
                tile.data = data;
                tile.gameObject.SetActive(true);
                return tile;
            }

            return null;
        }

        

        #endregion

        #region 关卡构成

        public void InitTiles(int[][] tiles, Sprite[] icons)
        {
            _tileIcons = icons;
            UI.StartCoroutine(OnLevelEnter(tiles));
        }

        private IEnumerator OnLevelEnter(int[][] tiles)
        {
            _enableClick = false;
            _activeList.Clear();
            float delay = 0;
            float moveTime = 0;
            string buff = "";
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[0].Length; j++)
                {
                    var data = Find(j, i);
                    if (data != null)
                    {
                        data.view.SetIcons(_tileIcons);
                        data.SetID(tiles[i][j]);
                        var pos = data.view.transform.localPosition;
                        delay = (i + j) * 0.01f;
                        moveTime = data.view.OnTileEnter(0, delay);
                        data.ClearMoveData();
                        _activeList.Add(data);
                    }
                }
            }
            yield return new WaitForSeconds(moveTime + 0.1f);
            yield return OnHandleResult();
            // _enableClick = true;
        }
        
        

        #endregion

        #region 数据查询

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="list"></param>
        /// <param name="isEmpty"></param>
        public void GetColumn(int colIndex, out List<TileData> list, out bool isEmpty)
        {
            isEmpty = false;
            list = new List<TileData>(Const.GRID_H);
            int i = 0;
            int empCount = 0;
            while (i < Const.GRID_H)
            {
                var t = Find(colIndex, i);
                if (t != null)
                {
                    if (t.IsDisabled) empCount++;
                    list.Add(t);
                }
                i++;
            }

            if (empCount == list.Count)
            {
                isEmpty = true;
            }
        }
        

        #endregion
        
        #region 点击消除

        /// <summary>
        ///  当点击Tile块时
        /// </summary>
        /// <param name="arg"></param>
        private void OnClickTile(object arg)
        {
            if (!_enableClick) return;
            OnMergeChecking((TileData) arg);
        }

        /// <summary>
        /// 生成一个排序用的序列
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int GetSortOrder(TileData a, TileData b)
        {
            var d = 100* (Mathf.Abs(a.y - b.y) + Mathf.Abs(a.x - b.x));
            if (a.x < b.x)
            {
                d -= 2;
            }
            else if (a.x > b.x)
            {
                d += 1;
            }
            
            if (a.y < b.y)
            {
                d -= 1;
            }
            else if (a.y > b.y)
            {
                d += 2;
            }
            
            return d;
        }
        
        /// <summary>
        /// 查询结果
        /// </summary>
        private int CheckResult()
        {
            _activeList.Clear();
            foreach (var t in _tiles.Values)
            {
                if(!t.IsDisabled) _activeList.Add(t);
            }
            
            var count = _activeList.Count;

            Debug.Log($"<color=orange>--- Remain Count: {count}</color>");
            
            if (count == 0) return RESULT_FINISH;
            if (count == 1) return RESULT_NOMOVE;

    
            for (int i = 1; i < count; i++)
            {
                _findList.Clear();
                FindMatchedTiles(_activeList[i]);
                if(_findList.Count > 1) return RESULT_GAMING;  // 有匹配就有解
            }
            // 最终块无解状态
            return RESULT_NOMOVE;
            
            // return RESULT_GAMING;
        }

        /// <summary>
        /// 合成判定
        /// </summary>
        /// <param name="center"></param>
        private void OnMergeChecking(TileData center)
        {
            if (null == center) return;
            
            _findList.Clear();
            _moveList.Clear();

            FindMatchedTiles(center);

            if (_findList.Count > 1)
            {
                _findList.Sort((a, b) =>
                {
                    if (GetSortOrder(a, center) < GetSortOrder(b, center)) return -1;
                    if (GetSortOrder(a, center) > GetSortOrder(b, center)) return 1;
                    return 0;
                });
                UI.StartCoroutine(OnTileMerge());
            }
            else
            {
                Debug.Log($"Not enough tile matched......");
            }
        }
        
        /// <summary>
        /// 查询匹配Tile
        /// </summary>
        /// <param name="data"></param>
        private void FindMatchedTiles(TileData data)
        {
            if (_findList.Contains(data)) return;
            
            _findList.Add(data);
            var tmp = data.CheckAround();
            for (int i = 0; i < tmp.Count; i++)
            {
                FindMatchedTiles(tmp[i]);
            }
        }
        
        /// <summary>
        /// 移动范围
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <returns></returns>
        private float TryDropTiles(int xmin, int xmax)
        {
            _moveList.Clear();
            var moveY = 0;
            for (int i = xmin; i <= xmax; i++)
            {
                moveY = 0;
                for (int j = Const.GRID_H-1; j >=0 ; j--)
                {
                    var t = Find(i, j);
                    if (null != t)
                    {
                        if (t.IsDisabled)
                        {
                            moveY++;
                        }
                        else 
                        {
                            if (moveY != 0)
                            {
                                t.moveY = moveY;
                                _moveList.Add(t);
                            }
                        }
                    }
                }
            }
            
            // 下落运动
            if (_moveList.Count > 0)
            {
                _moveList.Sort((a, b) =>
                {
                    if (a.OrderV > b.OrderV) return -1;
                    if (a.OrderV < b.OrderV) return 1;
                    return 0;
                });
                
                foreach (var t in _moveList)
                {
                    OnTileMove(t, Const.TILE_DROP_TIME, Ease.OutBack);
                }
                return Const.TILE_DROP_TIME;
            }
            return 0;
        }

        private float TryLookupLeft(int xmin)
        {
            var moveX = 0;
            _moveList.Clear();
            for (int i = xmin; i < Const.GRID_W; i++)
            {
                GetColumn(i, out var list, out var isEmpty);
                if (isEmpty)
                {
                    moveX--;
                    // Debug.Log($"==== FInd Empty Col: {i} ");
                }
                else
                {
                    if (moveX  != 0)
                    {
                        // 已有起始列
                        foreach (var t in list)
                        {
                            if (!t.IsDisabled)
                            {
                                t.moveX = moveX;
                                Debug.Log($"Left>>>{t.name} -> {t.moveX}");
                                _moveList.Add(t);
                            }
                        }
                    }
                }
            }

            
            // 左移运动
            if (_moveList.Count > 0)
            {
                _moveList.Sort((a, b) =>
                {
                    if (a.OrderH < b.OrderH) return -1;
                    if (a.OrderH > b.OrderH) return 1;
                    return 0;
                });
                
                foreach (var t in _moveList)
                {
                    OnTileMove(t);
                }

                return Const.TILE_MOVE_TIME;
            }
            return 0;
        }
        
        
        
        /// <summary>
        /// 方格合并和移动
        /// </summary>
        /// <returns></returns>
        IEnumerator OnTileMerge()
        {

            _enableClick = false;
            var xmin = 4;
            var xmax = 5;

            var sint = 0.06f;
            float stime = _findList.Count * Time.deltaTime;
            int scount = Mathf.CeilToInt(stime / sint);
            for (int i = 0; i < scount; i++)
            {
                Sound.Play(Sound.SFX.Merge, i*sint);
            }
            
            for (int i = 0; i < _findList.Count; i++)
            {
                var tile = _findList[i];
                if (tile.x < xmin) xmin = tile.x;
                else if (tile.x > xmax) xmax = tile.x;
                tile.SetDisable();
                tile.view.ShowMergeFX();
                AddScoreItem(i, tile, _scoreToPos);
                // yield return new WaitForSeconds(0.01f);
                yield return new WaitForEndOfFrame();
            }
            
            
            
            
            GEvent.Send(Events.OnShowMergeInfo.ToString());  // 显示合成信息

            // #1 先下落
            var res1 = TryDropTiles(xmin, xmax);
            yield return new WaitForSeconds(res1);
            // #2 再左对齐
            var res2 = TryLookupLeft(xmin);
            yield return new WaitForSeconds(res2 );
            // ------------------- 结果判定 -------------------------
            yield return OnHandleResult();
        }

        /// <summary>
        /// 对齐操作
        /// </summary>
        /// <param name="data"></param>
        /// <param name="time"></param>
        /// <param name="ease"></param>
        private void OnTileMove(TileData data, float time = 0, Ease ease = Ease.Linear)
        {
            var dest = Find((int) data.MovePos.x, (int) data.MovePos.y);
            if (time == 0) time = Const.TILE_MOVE_TIME;
            // Debug.Log($"--- OnTileMove ->{data.name}");
            if (dest != null)
            {
                TileData.Exchange(data, dest);

#if UNITY_EDITOR
                // Debug.Log($"<color=orange>{data.name} </color> offset: ({data.moveX}, {data.moveY}) --> <color=#88ff00>{dest.name}</color> :: [{data.id}]");
#endif

                var x = -data.moveX * Const.TILE_WITH;
                var y = data.moveY * Const.TILE_HEIGHT;
                data.ClearMoveData();
                dest.ClearMoveData();
                dest.view.MoveOffsetBack(x,y ,time , () =>
                {
                    // Debug.Log($"Move Over <color=#88ff00>{dest.name}: [{dest.id}]</color>");
                }, ease);
            }
            else
            {
                Debug.Log($"<color=red>Find DEST in pos ({data.x}, {data.y}) -> ({data.moveX},{data.moveY}) is Failed</color>");
            }
        }


        
        
        /// <summary>
        /// 设置游戏结束状态
        /// </summary>
        public void SetGameResult()
        {
            UI.StartCoroutine(OnHandleResult());
        }

        /// <summary>
        /// 处理结果
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnHandleResult()
        {
            switch (CheckResult())
            {
                case RESULT_GAMING:
                    _enableClick = true;
                    break;
                case RESULT_FINISH:
                    yield return new WaitForSeconds(1);
                    OnLevelFinished();
                    yield break;
                case RESULT_NOMOVE:
                    yield return OnUnsolved();
                    yield return new WaitForSeconds(1);
                    OnLevelFinished();
                    yield break;
            }
        }
        
        
        


        #endregion



        #region 分数效果
        
        /// <summary>
        /// 初始化分数对象
        /// </summary>
        private void InitScoreItems()
        {
            int count = 30;
            _scoreItems = new List<ScoreItem>(50);
            _scoreItems.Add(_scorePrefab.GetComponent<ScoreItem>());
            int i = 0;
            while (i < count)
            {
                var item = CreateScoreItem();
                item.Active = false;
                _scoreItems.Add(item);
                i++;
            }
            _scorePrefab.gameObject.SetActive(false);
        }

        private ScoreItem GetScoreItem()
        {
            if (_scoreItems.Count > 0)
            {
                var item = _scoreItems[0];
                _scoreItems.RemoveAt(0);
                item.Active = true;
                item.Score = 0;
                return item;
            }
            return CreateScoreItem();
        }

        private ScoreItem CreateScoreItem()
        {
            var go = GameObject.Instantiate(_scorePrefab, _scorePrefab.transform.parent);
            return go.GetComponent<ScoreItem>();
        }



        private void AddScoreItem(int index, TileData data, Vector3 dest)
        {
            var item = GetScoreItem();
            item.Position = data.view.Position;
            item.Score = (index +1) * Const.TILE_UNIT_SCORE;
            float time = 0.2f;
            var interval = 0.05f;
            var gap = 0.2f;
            if (index > 8)
            {
                interval = 0.01f;
                gap = 0.1f;
            }
            float delay = index * interval + gap;
            item.MoveFromTo(data.view.Position, dest, s =>
            {
                item.Active = false;
                _scoreItems.Add(item);
                OnAddScore?.Invoke(item.Score);
            }, time , delay);
        }


        #endregion

        #region 状态处理
        
        /// <summary>
        /// 关卡结束
        /// </summary>
        private void OnLevelFinished()
        {
            OnFinishLevel?.Invoke();
        }

        /// <summary>
        /// 处理无解情况
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnUnsolved()
        {
            float time = 1.5f;
            // 两种/三种最终无解的情况
            foreach (var t in _activeList)
            {
                t.view.SetFlash(time);
            }
            yield return new WaitForSeconds(time);
        }

        #endregion

        #region 提示处理


        /// <summary>
        /// 计算最大提示列表
        /// </summary>
        /// <returns></returns>
        private List<TileData> GetHintTileList()
        {
            var list = new List<TileData>(50);
            var all = new List<TileData>(_tiles.Values.ToList());
            while (all.Count > 0)
            {
                var c = all[0];
                all.Remove(c);
                if(c.IsDisabled) continue;
                _findList.Clear();
                FindMatchedTiles(c);
                if (_findList.Count > 1 && _findList.Count >list.Count)
                {
                    list.Clear();
                    foreach (var t in _findList)
                    {
                        list.Add(t);
                        all.Remove(t);
                    }
                }
            }
            return list;
        }
        
        /// <summary>
        /// 显示提示
        /// </summary>
        public void ShowHint()
        {
            var list = GetHintTileList();
            if (list.Count > 1)
            {
                _hintList = list;
                foreach (var t in _hintList)
                {
                    t.view.ShowHint();
                }
            }
        }
        
        /// <summary>
        /// 隐藏提示
        /// </summary>
        public void CancelHint()
        {
            if (_hintList == null || _hintList.Count < 2) return;
            foreach (var t in _hintList)
            {
                t.view.StopHint();
            }
        }
        
        
        
        

        #endregion

        #region 数据处理

        /// <summary>
        /// 转换为字符串数组
        /// </summary>
        /// <returns></returns>
        public List<UserTileData> ToUserTiles()
        {
            int count = _tiles.Count;
            List<UserTileData> list = new List<UserTileData>(count);
            var keys = _tiles.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                var tile = _tiles[keys[i]];
                list.Add(new UserTileData()
                {
                    x = tile.x,
                    y = tile.y,
                    id = tile.id,
                });
            }
            return list;
        }
        
        #endregion
    }
}