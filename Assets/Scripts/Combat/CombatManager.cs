using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using AFSInterview.Army;
using AFSInterview.Units;
using TMPro;
using UnityEngine;

namespace AFSInterview
{
	public class CombatManager : MonoBehaviour
	{
		[Header("References/UI")]
		[SerializeField] private TMP_Text[] turnIndicators = System.Array.Empty<TMP_Text>();

		[Header("References/Main")]
		[SerializeField] private Camera combatCamera = null;
		[SerializeField] private ArmyInstance[] armies = System.Array.Empty<ArmyInstance>();

		[Header("Settings")]
		[SerializeField] private LayerMask selectMask;

		private int currentArmyIndex = -1;
		private int turn = -1;

		private UnitInstance attacker = null;
		private UnitInstance defender = null;

		private void Start()
		{
			if(UnityEngine.Random.value < 0.5f)
				currentArmyIndex = 1;
			else
				currentArmyIndex = 0;

			NextTurn(); //for refresh

			foreach(var army in armies)
			{
				army.onUnitSelected += OnArmyUnitSelected;
			}
		}

		private void Update()
		{
			if(Input.GetMouseButtonDown(0)
			//Lock for not selecting units when attack lasts
			&& defender == null
			)
				TrySelectUnit();
		}

		private void OnArmyUnitSelected(UnitInstance unit, ArmyInstance army)
		{
			int found = Array.FindIndex(armies, a => a == army);
			if(found == currentArmyIndex)
			{
				if(!unit.TurnLock)
				{
					attacker = unit;

					turnIndicators[currentArmyIndex].SetText(
						$"Select enemy unit to attack!"
					);
				}
				else
				{
					turnIndicators[currentArmyIndex].SetText(
						$"Select another attacker, this one is exhaused!"
					);
				}
			}
			else if(attacker != null)
			{
				defender = unit; //for lock
				attacker.Attack(unit, () => {
					//unlock selection, process turn
					defender = null;
					attacker = null;

					NextTurn();
				});

				turnIndicators[currentArmyIndex].SetText(
					$"Attacking..."
				);
			}
		}

		//Rotational turns, allows fight between many armies
		public void NextTurn()
		{
			int prevArmy = currentArmyIndex;

			currentArmyIndex += 1;
			if(currentArmyIndex >= armies.Length)
				currentArmyIndex = 0;

			int idx = currentArmyIndex;
			//Let's assume that arrays are properly set up
			foreach(var txt in turnIndicators)
				txt.SetText(string.Empty);

			var army = armies[idx];
			if(army.Units.All(u => u.Data.HealthPoints <= 0))
			{
				turnIndicators[idx].SetText($"{army.Data.name} lost the battle!");
				//Would need to be adjusted when there is more than 2 armies!!!
				//Currently it locks the game on "lost screen"
			}
			else
			{
				turnIndicators[idx].SetText($"It's '{army.Data.name}' turn!");
			}

			turn += 1;

			//Telling units that turn has passed
			foreach(var a in armies)
				foreach(var unit in a.Units)
					unit.TurnPassed();
		}

		private void TrySelectUnit()
		{
			var ray = combatCamera.ScreenPointToRay(Input.mousePosition);
			if(!Physics.Raycast(ray, out var hit, 100f, selectMask)
			|| !hit.collider.TryGetComponent<UnitInstance>(out var unit)
			)
				return;

			unit.Data.Select();
		}
	}
}
