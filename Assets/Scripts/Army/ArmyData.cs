using System;
using System.Collections.Generic;
using AFSInterview.Units;
using UnityEngine;

namespace AFSInterview.Army
{
	[CreateAssetMenu(
		fileName = "ArmyData",
		menuName = "Combat/ArmyData"
	)]
	public class ArmyData : ScriptableObject
	{
		public IEnumerable<UnitData> Units => unitsData;
		public int UnitsCount => unitsData.Length;

		[SerializeField] private UnitData[] unitsData = System.Array.Empty<UnitData>();

		//Naive clone to not share array objects
		public ArmyData Clone()
		{
			var army = ScriptableObject.CreateInstance<ArmyData>();
			army.unitsData = new UnitData[UnitsCount];
			Array.Copy(unitsData, army.unitsData, UnitsCount);

			army.name = this.name;
			return army;
		}
	}
}
