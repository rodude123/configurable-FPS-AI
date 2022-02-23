using UnityEngine;
using utilities;

namespace Enemy.Configurators
{
    [CreateAssetMenu(menuName = "FPS AI System/Zombie AI Configuration", fileName = "zombieAI.asset")]
    public class ZombieAIConfiguration : AIConfiguration
    {
        public string walkParameterName;
        public bool canRun;
        [ConditionalHide("canRun", true)] public string runParameterName;
        public string attackParameterName;
        public bool canCrouch;
        [ConditionalHide("canCrouch", true)] public string crouchWalkParameterName;
        [HideInInspector] public new bool zombie = true;
    }
}