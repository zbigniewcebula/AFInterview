using UnityEngine;

namespace AFSInterview.Items
{
	//Please see: ItemUsage.cs (Command Pattern)
	[CreateAssetMenu(
		fileName = "ItemUsageDisposeItem",
		menuName = "ItemUsage/DisposeItem"
	)]
	public class ItemUsageDisposeItem : ItemUsage
	{
		public override void Use(Item item, InventoryController targetInventory)
		{
			targetInventory.RemoveItem(item);
		}
	}
}
