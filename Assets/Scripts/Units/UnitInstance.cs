using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace AFSInterview.Units
{
	public class UnitInstance : MonoBehaviour
	{
		public UnitData Data { get; private set; }
		public bool TurnLock => turnLock > 0;

		[Header("References")]
		[SerializeField] private Transform model;
		[SerializeField] private MeshRenderer render;
		[SerializeField] private new Collider collider;

		[Header("Settings")]
		[SerializeField] private float animTime;

		private int turnLock = 0;

		public void Setup(UnitData data)
		{
			Data = data.Clone();

			Data.onDeath += OnDeath;
			Data.onDamage += OnDamage;

			name = Data.name;

			Material newMat = new(render.sharedMaterial);
			newMat.color = Data.Color;
			render.sharedMaterial = newMat;
		}

		private void OnDeath(UnitData data)
		{
			collider.enabled = false;
			DeathAnimation(transform);
			Destroy(this, 0.5f);
		}

		private void OnDamage(UnitData data, int damage)
		{

		}

		//Primitive death animation, usually animator or DoTween should be considered
		//Why async? because this script will be deleted to not exist as Unit, but model should be a corpse
		private async void DeathAnimation(Transform toAnim)
		{
			var finalRot = Quaternion.Euler(0, 0, 90);
			model.rotation = Quaternion.Euler(0, 0, 0);

			//Naive waiting one frame
			float t = 0;
			while(t < animTime)
			{
				float delta = Time.deltaTime;
				await Task.Delay(Mathf.FloorToInt(delta * 1000));
				t += delta;

				model.rotation = Quaternion.Lerp(
					model.rotation, finalRot, t / animTime
				);
			}
			model.rotation = finalRot;
		}

		public void Attack(UnitInstance target, Action onAttackFinish = null)
		{
			turnLock = Data.AttackInterval;
			StartCoroutine(AttackAnimation(
				target.transform, () => Data.Attack(target.Data), onAttackFinish
			));
		}

		//Primitive attack animation, usually animator or DoTween should be considered
		private IEnumerator AttackAnimation(
			Transform attackedTarget, Action onTargetReach, Action onFinish = null
		)
		{
			Vector3 initialPos = transform.position;
			Vector3 targetPos = attackedTarget.position;

			float t = 0;
			while(t < animTime)
			{
				transform.position = Vector3.Lerp(
					initialPos, targetPos, t / animTime
				);
				yield return null;
				t += Time.deltaTime;
			}

			onTargetReach?.Invoke();

			t = 0;
			while(t < animTime)
			{
				transform.position = Vector3.Lerp(
					targetPos, initialPos, t / animTime
				);
				yield return null;
				t += Time.deltaTime;
			}
			
			onFinish?.Invoke();
		}

		public void TurnPassed()
		{
			if(turnLock > 0)
				--turnLock;
		}
	}
}
