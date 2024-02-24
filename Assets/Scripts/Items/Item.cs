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
		[SerializeField] private ItemUsage[] onUse;

		public string Name => name;
		public int Value => value;

		public Item(string name, int value)
		{
			this.name = name;
			this.value = value;
		}

		public void Use(InventoryController targetInventory)
		{
			Debug.Log($"Using: {Name}, Actions: {onUse.Length}");
			for(int i = 0; i < onUse.Length; ++i)
				onUse[i]?.Use(this, targetInventory);
		}
	}
}