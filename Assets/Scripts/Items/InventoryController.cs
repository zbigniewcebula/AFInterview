namespace AFSInterview.Items
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public class InventoryController : MonoBehaviour
	{
		[Header("Contents")]
		[SerializeField] private List<Item> items;
		[SerializeField] private int money;

#if UNITY_EDITOR
		//Primitive debug mechanism for inspector
		[Header("Debug\nEnter index of item and then check 'doUse'")]
		[SerializeField] private int itemToUse = -1;
		[SerializeField] private bool doUse = false;
#endif

		public int Money => money;
		public int ItemsCount => items.Count;

		public event Action onInventoryChange = null;
		public event Action onMoneyChange = null;

#if UNITY_EDITOR
		//Primitive debug mechanism for inspector
		//It would be better to implement PropertyDrawer with Button functionality
		//...but it's not the point of the task
		private void Update()
		{
			if(doUse)
			{
				UseItem(itemToUse);

				doUse = false;
				itemToUse = -1;
			}
		}
#endif

		public void SellAllItemsUpToValue(int maxValue)
		{
			/*	//Bugged approach
			for (var i = 0; i < items.Count; i++)
			{
				var itemValue = items[i].Value;
				if (itemValue > maxValue)
					continue;
				
				money += itemValue;
				items.RemoveAt(i);
			}
			*/

			//Non-Linq approach (most optimal)
			int initialCount = items.Count;
			for(int i = 0; i < items.Count; ++i)
			{
				var itemValue = items[i].Value;
				if(itemValue > maxValue)
					continue;

				money += itemValue;
				//change of collection causes leaps over some items
				items.RemoveAt(i);
				--i;
			}

			/*	//Linq approach (shortest and readable)
			money += items
				.Where(itm => itm.Value < maxValue)
				.Sum(itm => itm.Value);
			items.RemoveAll(itm => itm.Value < maxValue);
			*/

			int diff = initialCount - items.Count;
			Debug.Log($"Sold: {diff}");
			if(diff > 0)
			{
				onMoneyChange?.Invoke();
				onInventoryChange?.Invoke();
			}
		}

		public void AddItem(Item item)
		{
			items.Add(item);
			onInventoryChange?.Invoke();
		}
		public void RemoveItem(Item item)
		{
			items.Remove(item);
			onInventoryChange?.Invoke();
		}

		public void AddMoney(int count)
		{
			money += count;
			onMoneyChange?.Invoke();
		}

		public void UseItem(int itemIndex)
		{
			if(itemIndex >= 0 && itemIndex < items.Count)
				UseItem(items[itemIndex]);
		}
		private void UseItem(Item item)
		{
			item.Use(this);
		}
	}
}