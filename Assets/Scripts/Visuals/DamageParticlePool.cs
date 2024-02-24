using UnityEngine;
using UnityEngine.Pool;

namespace AFSInterview.Visuals
{
	/// <summary>
	/// Simple object pool, used internal Unity implementation
	///	The job of the pool is to avoid allocation/deallocation if it's not needed
	///	if object instance can be hold in memory and re-setup everytime is needed it's more
	///	optimized than constant re-allocation of object
	///
	///	In this case particles are needed only when Unit get's damage and disappears after it's lifetime
	///	instance doesn't need to be destroyed when we can simple re-setup label and reset position to
	///	new one of new damage Unit.
	///	
	/// PS It's also a singleton, but it's most common pattern that I think there is no need to explain
	/// I used it because of deadline, it would be better to setup Dependency Injection in the project
	/// and use Pool through that system
	/// </summary>
	public class DamageParticlePool : MonoBehaviour
	{
		private static DamageParticlePool instance;

		[Header("References")]
		[SerializeField] private DamageParticle praticlePrefab;

		private LinkedPool<DamageParticle> pool;

		private void Awake()
		{
			instance = this;
			pool = new(
				OnCreate, OnGet, OnRelease, null, true, 3
			);
		}
		private void OnDestroy()
		{
			instance = null;
		}

		public static DamageParticle Spawn(Vector3 atPos, int damage)
		{
			var ret = instance.pool.Get();
			ret.transform.position = atPos;
			ret.Setup(damage, instance.pool.Release);
			return ret;
		}

		private DamageParticle OnCreate()
		{
			return Instantiate(praticlePrefab, transform);
		}
		private void OnGet(DamageParticle particle)
		{
			particle.gameObject.SetActive(true);
		}
		private void OnRelease(DamageParticle particle)
		{
			particle.gameObject.SetActive(false);
		}
	}
}
