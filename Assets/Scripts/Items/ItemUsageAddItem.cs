using UnityEngine;

namespace AFSInterview.Items
{
	//Please see: ItemUsage.cs (Command Pattern)
	[CreateAssetMenu(
		fileName = "ItemUsageAddItem",
		menuName = "ItemUsage/AddItem"
	)]
	public class ItemUsageAddItem : ItemUsage
	{
		[Header("References")]
		[SerializeField]
		private Item toAdd = null;

		public override void Use(Item item, InventoryController targetInventory)
		{
			if(toAdd != null)
				targetInventory.AddItem(toAdd);
		}
	}
}
