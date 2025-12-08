using System;
using System.Collections;
using DG.Tweening;
using GKIT.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Tile: MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Text _txtId;
        [SerializeField] private GameObject _fxStar;
        
        public TileData data;
        private Sprite[] _icons;
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private Sequence _curSequ;

#if UNITY_EDITOR
        private bool _showInfo = false;
#endif

        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        #region 初始化

        private void Awake()
        {
            _txtId.gameObject.SetActive(false);
            _fxStar.SetActive(false);
        }

        public void StopMove()
        {
            _icon.DOKill();
            _icon.transform.DOKill();
            _icon.transform.localScale = Vector3.one;
            if (_curSequ != null)
            {
                _curSequ.Kill();
                _curSequ = null;
            }
        }

        #endregion
        
        #region 点击行为

        public void OnPointerDown(PointerEventData eventData)
        {
            if (data.IsDisabled) return;  // 空格不可点击
            
            Debug.Log($"Tile {name} has been clicked");
            GEvent.Send(Events.OnClickTile.ToString(), data);
        }

        #endregion

        #region 闪烁

        public void SetFlash(float time = 0)
        {
            if (time == 0) time = 1.5f;
            StopMove();
            _curSequ = DOTween.Sequence()
                .Append(_icon.DOFade(0, 0.02f).SetDelay(time * 0.3f))
                .Append(_icon.DOFade(1, 0.02f).SetDelay(time * 0.3f))
                .Append(_icon.DOFade(0, 0.02f).SetDelay(time * 0.2f))
                .Append(_icon.DOFade(1, 0.02f).SetDelay(time * 0.1f))
                .Append(_icon.DOFade(0, 0.02f).SetDelay(time * 0.1f));
            _curSequ.Play();
        }

        #endregion

        #region 位置和移动

        public void SetPos(float x, float y)
        {
            transform.localPosition = new Vector3(x, y, 0);
        }
        

        public void SetIconPos(float x, float y)
        {
            _icon.transform.localPosition = new Vector2(x, y);
        }

        public void ResetIconPos()
        {
            SetIconPos(0, 0);
        }

        /// <summary>
        /// 将图标移动到
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="time"></param>
        /// <param name="callback"></param>
        public void MoveIcon(float x, float y, float time, Action callback, Ease ease = Ease.OutBack, float delay = 0)
        {
            StopMove();
            _icon.rectTransform.DOLocalMove(new Vector3(x, y,0), time)
                .SetEase(ease)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                });
        }

        /// <summary>
        /// 预先设置ICON偏移量, 再将ICON移动回中心点
        /// </summary>
        /// <param name="tx">X偏移量, 按格子算</param>
        /// <param name="ty">Y偏移量, 按格子算</param>
        /// <param name="time"></param>
        /// <param name="callback"></param>
        /// <param name="ease"></param>
        public void MoveOffsetBack(float tx, float ty, float time, Action callback, Ease ease = Ease.Linear, float delay = 0)
        {
            SetIconPos(tx, ty);
            MoveIcon(0, 0, time, () =>
            {
                callback?.Invoke();
            });
        }

        /// <summary>
        /// 方块入场
        /// </summary>
        public float OnTileEnter(float time = 0, float delay = 0)
        {
            StopMove();
            if (time == 0) time = Const.TILE_ENTER_TIME;
            var ty = -transform.localPosition.y + 1200+ 100;
            // var ty = Screen.height;
            SetIconPos(0, ty);
            _curSequ = DOTween.Sequence()
                .Append(_icon.transform.DOLocalMoveY(0, time * 1f))
                .Append(_icon.transform.DOLocalMoveY(16, time * 0.2f))
                .Append(_icon.transform.DOLocalMoveY(-8, time * 0.1f))
                .Append(_icon.transform.DOLocalMoveY(6, time * 0.1f))
                .Append(_icon.transform.DOLocalMoveY(0, time * 0.1f))
                .SetDelay(delay);
            _curSequ.Play();
            return delay + time;
        }
        
        
        #endregion

        #region 设置图标
        
        /// <summary>
        /// 设置图表库
        /// </summary>
        /// <param name="icons"></param>
        public void SetIcons(Sprite[] icons)
        {
            _icons = icons;
        }

        /// <summary>
        /// 设置图标
        /// </summary>
        /// <param name="index"></param>
        public void SetIcon(int index)
        {
            if (index < 0)
            {
                _icon.gameObject.SetActive(false);
                return;
            }
            
            _icon.gameObject.SetActive(true);
            _icon.color = Color.white;
            if (_icons != null && index < _icons.Length)
            {
                if (index >= _icons.Length) index = _icons.Length - 1;
                _icon.sprite = _icons[index];
            }
          
        }

        #endregion

        #region 更新数据

        public void UpdateInfo()
        {
            gameObject.name =  $"t_{data.x}_{data.y} ({data.id})";
#if UNITY_EDITOR
            if (_showInfo)
            {
                _txtId.gameObject.SetActive(true);
                _txtId.text = gameObject.name;
            }
#endif
            
            
        }
        

        #endregion

        #region 提示效果

        /// <summary>
        /// 显示提示
        /// </summary>
        public void ShowHint(float time = 0)
        {
            if (time == 0) time = Const.TILE_HINT_TIME;
            StopMove();
            _curSequ = DOTween.Sequence()
                .Append(_icon.transform.DOLocalMoveY(-2, time*0.2f ))
                .Join(_icon.transform.DOScale(new Vector3(1.1f, 0.9f, 1), time*0.2f ))
                .Append(_icon.transform.DOLocalMoveY(20, time*0.2f ))
                .Join(_icon.transform.DOScale(new Vector3(0.9f, 1.1f, 1), time*0.2f ))
                .Append(_icon.transform.DOLocalMoveY(-4, time*0.15f ))
                .Join(_icon.transform.DOScale(new Vector3(1.05f, 0.95f, 1), time*0.1f ))
                .Append(_icon.transform.DOLocalMoveY(12, time*0.15f ))
                .Join(_icon.transform.DOScale(new Vector3(0.95f, 1.05f, 1), time*0.1f ))
                .Append(_icon.transform.DOLocalMoveY(0, time*0.1f ))
                .Join(_icon.transform.DOScale(new Vector3(1f, 1f, 1), time*0.1f ))
                .AppendInterval(time * 0.2f)
                .SetLoops(-1, LoopType.Restart);
            _curSequ.Play();
        }


        public void StopHint()
        {
            StopMove();
            _icon.transform.localPosition = Vector3.zero;
            _icon.transform.localScale = Vector3.one;
        }

        #endregion

        #region 显示特效

        public void ShowMergeFX()
        {
            StartCoroutine(nameof(OnMergeFX));
        }

        IEnumerator OnMergeFX()
        {
            _fxStar.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _fxStar.SetActive(false);
        }

        #endregion
    }
}