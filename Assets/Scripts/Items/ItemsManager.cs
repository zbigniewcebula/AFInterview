namespace AFSInterview.Items
{
	using System.Collections;
	using TMPro;
	using UnityEngine;

	public class ItemsManager : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private TMP_Text moneyUILabel = null;
		[SerializeField] private InventoryController inventoryController;
		[SerializeField] private Transform itemSpawnParent;
		[SerializeField] private GameObject itemPrefab;
		[SerializeField] private BoxCollider itemSpawnArea;
		[SerializeField] private Camera viewCamera;

		[Header("Settings")]
		[SerializeField] private LayerMask pickupMask;
		[SerializeField] private int itemSellMaxValue;
		[SerializeField] private float itemSpawnInterval;

		//In case of Async/Await approach
		//private CancellationTokenSource cancellationToken = new();

		private void Start()
		{
			inventoryController.onMoneyChange += OnMoneyChange;
			OnMoneyChange();

			//moved this code from Update(), it's unnecessary to be updated each frame
			StartCoroutine(SpawnItemRoutine());
		}

		private void OnDestroy()
		{
			//In case of Async/Await approach
			//cancellationToken.Cancel();
		}

		private void Update()
		{
			if(Input.GetMouseButtonDown(0))
				TryPickUpItem();

			if(Input.GetKeyDown(KeyCode.Space))
				inventoryController.SellAllItemsUpToValue(itemSellMaxValue);
		}

		private void OnMoneyChange()
		{
			//it would be nice to do nullcheck
			//or
			//use some kind of [Required] attribute to ensure that inspector warns developer of not bound reference
			//	I assume that developer (me) does the reference binding in the inspector right after compilation in Unity
			moneyUILabel.SetText($"Money: {inventoryController.Money}");
		}

		private IEnumerator SpawnItemRoutine()
		{
			var waiter = new WaitForSeconds(itemSpawnInterval);
			while(true)
			{
				SpawnNewItem();
				yield return waiter;
			}
		}
		/*	//Async/Await approach
		private async void SpawnItemTask()
		{
			while(!cancellationToken.IsCancellationRequested)
			{
				SpawnNewItem();
				await Task.Delay(Mathf.FloorToInt(itemSpawnInterval * 1000));
			}
		}
		*/

		private void SpawnNewItem()
		{
			var spawnAreaBounds = itemSpawnArea.bounds;
			Vector3 position = new(
				Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
				0f,
				Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
			);

			Instantiate(itemPrefab, position, Quaternion.identity, itemSpawnParent);
		}

		private void TryPickUpItem()
		{
			var ray = viewCamera.ScreenPointToRay(Input.mousePosition);
			if(!Physics.Raycast(ray, out var hit, 100f, pickupMask)
			|| !hit.collider.TryGetComponent<IItemHolder>(out var itemHolder)
			)
				return;

			var item = itemHolder.GetItem(true);
			inventoryController.AddItem(item);
			Debug.Log($"Picked up {item.Name} with value of {item.Value} and now have {inventoryController.ItemsCount} items");
		}
	}
}