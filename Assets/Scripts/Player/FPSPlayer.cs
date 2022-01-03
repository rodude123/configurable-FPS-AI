using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public abstract class FPSPlayer: MonoBehaviour
    {
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