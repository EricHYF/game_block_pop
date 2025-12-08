using DG.Tweening;
using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class UIMergeInfo: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected Image word1;
		protected Image word2;
		protected Image word3;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			word1 = FindUI<Image>("word_1");
			word2 = FindUI<Image>("word_2");
			word3 = FindUI<Image>("word_3");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}


		private Image[] _words;
		private Sequence _queue;
		private Image _curWord;
		private Color _fadeColor = new Color(1,1,1,0);
		
		protected override void Init()
		{
			base.Init();
			_words = new Image[3];
			word1.color = _fadeColor;
			word2.color = _fadeColor;
			word3.color = _fadeColor;
			
			_words[0] = word1;
			_words[1] = word2;
			_words[2] = word3;

		}


		public void Show()
		{
			_queue?.Kill();
			if (_curWord != null) _curWord.color = _fadeColor;

			root.localPosition = new Vector3(0, -100, 0);

			int idx = Random.Range(0, _words.Length);

			switch (idx)
			{
				case 0:
					Sound.Play(Sound.SFX.FXCool);
					break;
				case 1:
					Sound.Play(Sound.SFX.FXGreat);
					break;
				case 2:
					Sound.Play(Sound.SFX.FXExcellent);
					break;
			}
			
			_curWord = _words[idx];
			_curWord.color = _fadeColor;

			_queue = DOTween.Sequence()
				.Append(root.DOLocalMoveY(0, 0.4f))
				.Join(_curWord.DOFade(1, 0.4f))
				.AppendInterval(0.6f)
				.Append(_curWord.DOFade(0, 0.4f))
				.SetAutoKill(true)
				.OnComplete(() =>
				{
					_queue = null;
					
				});
			_queue.Play();
		}


    }
}
