using System.Collections;
using System.Collections.Generic;
using TMPro;
using GKIT.UI;
using GKIT;
using UnityEngine.UI;
using UnityEngine;
namespace Game
{
    public class UIBulletScreen: ViewBase
    {
		//********** [CODE GEN] PROPERTY DEFINE HEAD **********//
		protected RectTransform root;
		protected RectTransform top;
		protected RectTransform bot;
		protected RectTransform bulletItem;
		//********** [CODE GEN] PROPERTY DEFINE END **********//
	    
		protected override void OnCreateOver()
		{
			//********** [CODE GEN] PROPERTY STATEMENT HEAD **********//
			base.OnCreateOver(); // 一定要调用基类的方法, 会有一些基础组件的初始化
			root = FindUI<RectTransform>("root");
			top = FindUI<RectTransform>("top");
			bot = FindUI<RectTransform>("bot");
			bulletItem = FindUI<RectTransform>("bullet_item");
			//********** [CODE GEN] PROPERTY STATEMENT END **********//
		}



		public bool RootActive
		{
			get => root.gameObject.activeSelf;
			set => root.gameObject.SetActive(value);
		}

		private Stack<UIBulletScreenItem> _itemCache;
		private List<UIBulletScreenItem> _items;
		private float _moveTime;
		private float _interval;
		private int _dir = 1;
		private int _bulletIndex = 0;
		private bool _isRunning;
		
		
		
		#region 生命周期
		
		/// <summary>
		/// 初始化
		/// </summary>
		protected override void Init()
		{
			base.Init();

			RootActive = false;
			_moveTime = 10;
			_interval = 3;
			_dir = 1;
			_bulletIndex = 0;
			_isRunning = false;
			InitBulletItems();
		}

		private void InitBulletItems()
		{
			_items = new List<UIBulletScreenItem>(5);
			_itemCache = new Stack<UIBulletScreenItem>(5);
			int i = 0;
			while (i < 3)
			{
				var item = GetItem();
				if (item != null)
				{
					OnSaveItem(item);
				}
				i++;
			}
			bulletItem.gameObject.SetActive(false);
		}

		private UIBulletScreenItem GetItem()
		{
			if (_itemCache.Count > 0)
			{
				return _itemCache.Pop();
			}

			var go = Object.Instantiate(bulletItem.gameObject, root);
			if (go != null)
			{
				var item = AddSubUI<UIBulletScreenItem>(go.transform);
				if (item != null)
				{
					item.InitWithData(_moveTime, OnSaveItem);
					item.Active = true;
				}
				return item;
			}

			return null;
		}

		/// <summary>
		/// 存储Item
		/// </summary>
		/// <param name="item"></param>
		private void OnSaveItem(UIBulletScreenItem item)
		{
			item.OnSaved();
			if (_items.IndexOf(item) > -1)
			{
				_items.Remove(item);
			}

			item.Active = false;
			_itemCache.Push(item);
		}
		
		
		#endregion

		#region 公开接口

		/// <summary>
		/// 显示弹幕
		/// </summary>
		public void Show()
		{
			RootActive = true;
			StartBulletScreen();
		}

		/// <summary>
		/// 隐藏弹幕
		/// </summary>
		public void Hide()
		{
			RootActive = false;
		}

		#endregion

		#region 显示弹幕


		private string GetRandName()
		{
			var n = Random.Range(1001, 9999);
			return $"1***{n}";
		}

		private string GetRandValue()
		{
			var r = Random.Range(0, 100);
			if(r <= 2 )
			{
				return "2000";
			}
			else if (r <= 5)
			{
				return "1000";
			}
			else if (r <= 60)
			{
				return "300";
			}
			else
			{
				return "0.3";
			}
		}

		private int GetNextIndex()
		{
			_bulletIndex++;
			if (_bulletIndex >= int.MaxValue) _bulletIndex = 0;
			return _bulletIndex;
		}

		/// <summary>
		/// 显示下条弹幕
		/// </summary>
		private void ShowNextBullet()
		{
			var item = GetItem();
			var pos = _dir == 1 ? top.localPosition : bot.localPosition;
			_dir *= -1;
			item.ResetMoveInfo(GetRandName(), GetRandValue(), pos);
			item.Name = $"item_{GetNextIndex()}";
			_items.Add(item);
		}

		/// <summary>
		/// 缓存所有的弹幕
		/// </summary>
		private void SaveAllBullets()
		{
			while (_items.Count > 0)
			{
				OnSaveItem(_items[0]);
			}
			_items.Clear();
		}

		private void StartBulletScreen()
		{
			if (_isRunning) return;
			StartCoroutine(OnBulletRunning());
		}

		private IEnumerator OnBulletRunning()
		{
			_isRunning = true;
			while (_isRunning)
			{
				yield return new WaitForSeconds(_interval);
				ShowNextBullet();
			}
		}


		private void StopBulletScreen()
		{
			_isRunning = false;
			SaveAllBullets();
		}

		#endregion
		
    }
}
