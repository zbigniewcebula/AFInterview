using UnityEngine;

namespace AFSInterview.Items
{
	/// <summary>
	/// ItemUsage is a command pattern which encapsulates functionality in form of class instance
	/// Using this along with ScriptableObjects we can provide simple "Parametrized commands" that are
	/// bindable with internal Unity interface of inspector
	/// they are also serialized that helps us with storing data needed to execute specified command by
	/// executor, in this case exexuting context is in inventory when item is used
	/// </summary>
	public abstract class ItemUsage : ScriptableObject, IItemUsable
	{
		//ScriptableObject for sake of parametrization
		public virtual void Use(Item item, InventoryController targetInventory)
			=> throw new System.NotImplementedException();
	}

	public interface IItemUsable
	{
		//Due to lack of DI and no need for singletons,
		//let's inject inventory during usage
		public void Use(Item item, InventoryController targetInventory);
	}
}
