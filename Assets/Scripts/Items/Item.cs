namespace AFSInterview.Items
{
	using System;
	using UnityEngine;

	[Serializable]
	public class Item
	{
		[SerializeField] private string name;
		[SerializeField] private int value;
		//Please see: ItemUsage.cs (Command Pattern)
		[SerializeField] private ItemUsage[] onUse = System.Array.Empty<ItemUsage>();

		public string Name => name;
		public int Value => value;

		public Item(string name, int value, ItemUsage[] onUse = null)
		{
			this.name = name;
			this.value = value;
			this.onUse = onUse;
		}

		public void Use(InventoryController targetInventory)
		{
			Debug.Log($"Using: {Name}, Actions: {onUse.Length}");
			for(int i = 0; i < onUse.Length; ++i)
				onUse[i]?.Use(this, targetInventory);
		}
	}
}