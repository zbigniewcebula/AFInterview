using System;
using UnityEngine;

namespace AFSInterview.Units
{
	[CreateAssetMenu(
		fileName = "UnitData",
		menuName = "Combat/UnitData"
	)]
	public class UnitData : ScriptableObject
	{
		public int ArmorPoints { get; private set; }
		public int HealthPoints { get; private set; }
		public int AttackInterval => attackInterval;
		public Color Color => color;

		[Header("Visuals")]
		[SerializeField] private Color color = Color.white;

		[Header("Settings")]
		[SerializeField] private UnitAttributes attributes = UnitAttributes.None;
		[SerializeField] private int startHP = 100;
		[SerializeField] private int startAP = 20;

		[Header("Settings/Attack")]
		[SerializeField] private int attackInterval = 1;
		[SerializeField] private int attackDamage = 10;
		[SerializeField]
		private DamageOverride[] damageOverrides = System.Array.Empty<DamageOverride>();

		public event Action<UnitData> onDeath = null;
		public event Action<UnitData, int> onDamage = null;
		public event Action<UnitData> onSelected = null;

		private void OnEnable()
		{
			HealthPoints = startHP;
			ArmorPoints = startAP;
		}

		public void Attack(UnitData target)
		{
			int baseDMG = attackDamage;

			//Due to lack of description what happens when overide overlaps multiple attributes
			//This part of code assumes that first is the best
			//In case of prioritization, for example: "find most damage value among fitting overides"
			//	there is possibility to sort overrides and find the fitting one
			//Also using Array built-in mechanism to not overuse Linq which is not optimal for such simple cases
			var dmgOverride = Array.Find(
				damageOverrides, o => (o.againstWhom & target.attributes) != 0
			);
			if(dmgOverride != null)
				baseDMG = dmgOverride.damage;

			int finalDMG = baseDMG - target.ArmorPoints;
			if(finalDMG < 1)
				finalDMG = 1;

			target.GetDamage(finalDMG);
		}

		private void Die()
		{
			onDeath?.Invoke(this);
		}

		public void GetDamage(int damage)
		{
			HealthPoints -= damage;
			if(HealthPoints <= 0)
			{
				HealthPoints = 0;
				Die();
			}
			onDamage?.Invoke(this, damage);
		}

		public void Select()
		{
			onSelected?.Invoke(this);
		}

		public UnitData Clone()
		{
			//To not overcomplicate shallow copy, using MemberWise
			return this.MemberwiseClone() as UnitData;

			/*	//To be sure that we copy only things that we want:
			//	It could be done also by manualy writing code
			//	or
			//	reflections that keep this process more automatic when adding new properties to the class
			UnitData ret = ScriptableObject.CreateInstance<UnitData>();
			var fields = typeof(UnitData).GetFields(
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
			).Where(f => f.GetCustomAttribute<SerializeField>() != null);
			foreach(var field in fields)
				field.SetValue(ret, field.GetValue(this));
			return ret;
			*/
		}

		/// <summary>
		/// Entry that describes override of damage when THIS unity attacks OTHER unity of given attribute
		/// </summary>
		public class DamageOverride
		{
			public readonly UnitAttributes againstWhom;
			public readonly int damage;
		}
	}
}
