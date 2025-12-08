using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    
    /// <summary>
    /// 分数对象
    /// </summary>
    public class ScoreItem: MonoBehaviour
    {
        [SerializeField] private Text _txtScore;
        [SerializeField] private RectTransform _root;

        private int _score;
        /// <summary>
        /// 分数对象
        /// </summary>
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                _txtScore.text = $"{_score}";
            }
        }

        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 LocalPos
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }



        #region 移动

        /// <summary>
        /// 移动到位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="callback"></param>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        public void MoveTo(Vector3 pos, Action<ScoreItem> callback, float time = 0, float delay = 0)
        {
            Active = true;
            if (time == 0) time = Const.SCORE_FLY_TIME;
            var seq = DOTween.Sequence();
            
            if (delay > 0)
            {
                transform.localScale = Vector3.zero;
                seq.Append(transform.DOScale(1, delay).SetEase(Ease.OutBack));
            }

            seq.Append(transform.DOMove(pos, time));
            seq.OnComplete(() => callback?.Invoke(this));
            seq.Play();
        }
        
        /// <summary>
        /// 点移动
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="callback"></param>
        /// <param name="time"></param>
        public void MoveFromTo(Vector3 from, Vector3 to, Action<ScoreItem> callback, float time = 0, float delay = 0)
        {
            transform.position = from;
            MoveTo(to, callback, time, delay);
        }

        #endregion
        
       

    }
}