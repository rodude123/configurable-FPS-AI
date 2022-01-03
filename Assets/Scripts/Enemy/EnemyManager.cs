using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using utilities;


namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Game options")]
        public FPSPlayer player;
        
        public bool spawnMode;
        [ConditionalHide("spawnMode")]
        public List<GameObject> enemiesToSpawn;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}