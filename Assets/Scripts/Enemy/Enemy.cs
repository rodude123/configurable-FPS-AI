using System;
using Enemy.Configurators;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider))]
    [AddComponentMenu("FPS AI System/Enemy AI")]
    public class Enemy : MonoBehaviour
    {
        [Header("Configuration")] public AIConfiguration aiConfig;
        
        private FPSPlayer player;
        private NavMeshAgent agent;
        
        void Start()
        {
            player = EnemyManager.Instance.player;
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (aiConfig.zombie)
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }
}