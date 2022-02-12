using System;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider))]
    public class Enemy : MonoBehaviour
    {
        [Header("Configuration")]
        public bool zombie;
        
        private FPSPlayer player;
        private NavMeshAgent agent;
        
        void Start()
        {
            player = EnemyManager.Instance.player;
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (zombie)
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }
}