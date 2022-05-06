using UnityEngine;

namespace Player
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

        public float health
        {
            get;
            set;
        }

		public abstract void TakeDamage(float damage);

		public abstract void Shoot();
	}
}
