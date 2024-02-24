using UnityEngine;

namespace AFSInterview.Items
{
	//Please see: ItemUsage.cs (Command Pattern)
	[CreateAssetMenu(
		fileName = "ItemUsageAddMoney",
		menuName = "ItemUsage/AddMoney"
	)]
	public class ItemUsageAddMoney : ItemUsage
	{
		[Header("Settings")]
		[SerializeField]
		private int toAdd = 0;

		public override void Use(Item item, InventoryController targetInventory)
		{
			targetInventory.AddMoney(toAdd);
		}
	}
}
