using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public abstract class FPSPlayer: MonoBehaviour
    {
        public float health = 100f;
        public enum WeaponType
        {
            Pistol,
            MachineGun,
            Shotgun,
            Sniper,
        }
        public abstract void takeDamage(int damage);

        public abstract void shoot();
    }
}