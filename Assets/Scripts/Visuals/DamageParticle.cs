using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace AFSInterview.Visuals
{
	//Please see: DamageParticlePool.cs (Object Pool Pattern)
	public class DamageParticle : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private TMP_Text label;

		[Header("Settings")]
		[SerializeField] private float lifetime = 3f;
		[SerializeField] private float ascendingSpeed = 2f;

		public void Setup(int damage, Action<DamageParticle> onEnd)
		{
			label.SetText(damage.ToString());
			StartCoroutine(LifeTimeRoutine(onEnd));
		}

		private IEnumerator LifeTimeRoutine(Action<DamageParticle> onEnd)
		{
			yield return new WaitForSeconds(lifetime);
			onEnd?.Invoke(this);
		}

		void Update()
		{
			transform.Translate(
				0, Time.deltaTime * ascendingSpeed, 0
			);
		}
	}
}
