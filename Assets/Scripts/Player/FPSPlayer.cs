using UnityEngine;

namespace Assets.Scripts.Player
{
	public abstract class FPSPlayer : MonoBehaviour
	{
		public enum WeaponType
		{
			Pistol,
			MachineGun,
			Shotgun,
			Sniper
		}
		public float health = 100f;

		public abstract void TakeDamage(float damage);

		public abstract void Shoot();
	}
}