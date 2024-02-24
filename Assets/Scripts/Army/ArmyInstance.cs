using System;
using System.Collections.Generic;
using AFSInterview.Units;
using UnityEngine;

namespace AFSInterview.Army
{
	public class ArmyInstance : MonoBehaviour
	{
		public ArmyData Data { get; private set; }
		public IEnumerable<UnitInstance> Units => units;

		[Header("References")]
		[SerializeField] private UnitInstance unitPrefab;
		[SerializeField] private ArmyData initialData;

		public event Action<UnitInstance, ArmyInstance> onUnitSelected = null;

		private List<UnitInstance> units = new();

		private void Awake()
		{
			Data = initialData.Clone();

			int i = 0;
			int count = Data.UnitsCount;
			foreach(var unit in Data.Units)
			{
				UnitInstance instance = Instantiate(
					unitPrefab, default, Quaternion.identity, transform
				);
				instance.Setup(unit);
				instance.Data.onDeath += OnUnitDeath;
				instance.Data.onSelected += OnUnitSelected;

				//Scatter units randomly around army center (overlaping looks bad)
				instance.transform.position = transform.position + new Vector3(
					i%2 == 0? -1: 1,
					0f,
					i * 1.5f - count / 2f
				);

				units.Add(instance);
				++i;
			}
		}

		private void OnUnitDeath(UnitData data)
		{
			//Unit removal from list
			for(int i = 0; i < units.Count; ++i)
			{
				if(units[i].Data == data)
				{
					units[i].Data.onDeath -= OnUnitDeath;
					units.RemoveAt(i);
					return;
				}
			}
		}

		private void OnUnitSelected(UnitData data)
		{
			var unit = units.Find(u => u.Data == data);
			onUnitSelected?.Invoke(unit, this);
		}
	}
}
