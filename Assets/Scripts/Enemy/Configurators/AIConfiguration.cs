using UnityEngine;

namespace Enemy.Configurators
{
    public abstract class AIConfiguration : ScriptableObject
    {
        [HideInInspector] public bool zombie;
    }
}