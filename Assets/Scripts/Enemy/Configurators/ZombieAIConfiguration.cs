using UnityEngine;

namespace Assets.Scripts.Enemy.Configurators
{
	[CreateAssetMenu(menuName = "FPS AI System/Zombie AI Configuration", fileName = "zombieAI.asset")]
	public class ZombieAIConfiguration : ScriptableObject
	{
		public string walkParameterName;

		public string attackParameterName;

		[Range(0.1f, 20f)]
		[Tooltip("Attack Range of the enemy")]
		public float attackRange;

		[Range(0.1f, 20f)]
		[Tooltip("Damage dealt to the player per hit")]
		public float damage = 2f;

		[Range(1f, 20f)]
		[Tooltip("The attack delay of the enemy")]
		public double attackDelay = 1d;

		public string dieParameterName;
	}
}