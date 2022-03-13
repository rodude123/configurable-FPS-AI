using System.Collections;
using System.Collections.Generic;
using Enemy.Configurators;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(Animator))]
	[AddComponentMenu("FPS AI System/Enemy AI")]
	public class Enemy : MonoBehaviour
	{
		[Header("Configuration")]
		public ZombieAIConfiguration zombieAiConfig;

		public float health = 100f;
		private NavMeshAgent _agent;
		private Animator _anim;

		private FPSPlayer _player;

		private EnemyManager _enemyManager;
		private double _previousAttackTime;

		private void Start()
		{
			_enemyManager = EnemyManager.Instance;
			_player = _enemyManager.player;
			_agent = GetComponent<NavMeshAgent>();
			_anim = GetComponent<Animator>();
		}

		private void Update()
		{
			if (zombieAiConfig == null)
			{
				return;
			}

			_agent.SetDestination(_player.transform.position);
			_anim.SetBool(zombieAiConfig.walkParameterName, true);

			if (Vector3.Distance(_player.transform.position, transform.position) < zombieAiConfig.attackRange)
			{
				//attacking
				_anim.SetBool(zombieAiConfig.attackParameterName, true);
				if (Time.time - _previousAttackTime > zombieAiConfig.attackDelay)
				{
					_previousAttackTime = Time.time;
					_player.TakeDamage(zombieAiConfig.damage);
				}
			}
			else
			{
				// anim.SetBool(zombieAiConfig.walkParameterName, true);
				_anim.SetBool(zombieAiConfig.attackParameterName, false);
			}
		}

		public void TakeDamage(float damage)
		{
			health -= damage;
			if (!(health <= 0))
			{
				return;
			}
			_anim.SetBool(zombieAiConfig.dieParameterName, true);
			_agent.isStopped = true;
			StartCoroutine(nameof(Disable));
			Debug.Log("Enemy died");
		}

		private IEnumerator Disable()
		{
			yield return new WaitForSeconds(15f);
			gameObject.SetActive(false);
		}

		public void OnEnable()
		{
			gameObject.SetActive(true);
			_agent.isStopped = false;
			_agent.SetDestination(_player.transform.position);
			transform.position = _enemyManager.spawnPoints[Random.Range(0, _enemyManager.spawnPoints.Count)].transform.position;
		}

	}
}