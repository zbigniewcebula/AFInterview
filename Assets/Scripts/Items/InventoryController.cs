namespace AFSInterview.Items
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public class InventoryController : MonoBehaviour
	{
		[SerializeField] private List<Item> items;
		[SerializeField] private int money;

		public int Money => money;
		public int ItemsCount => items.Count;

		public event Action onInventoryChange = null;
		public event Action onMoneyChange = null;
		
		public void SellAllItemsUpToValue(int maxValue)
		{
			for (var i = 0; i < items.Count; i++)
			{
				var itemValue = items[i].Value;
				if (itemValue > maxValue)
					continue;
				
				money += itemValue;
				items.RemoveAt(i);
			}
		}

		public void AddItem(Item item)
		{
			items.Add(item);
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