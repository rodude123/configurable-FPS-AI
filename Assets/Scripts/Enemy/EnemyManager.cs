using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using utilities;
using Random = System.Random;


namespace Enemy
{
    
    [Serializable]
    public enum SpawnSystem
    {
        Continuous,
        RoundBased
    }
    
    public class EnemyManager : MonoBehaviour
    {
        [Header("Enemy options")]
        public FPSPlayer player;
        
        public bool spawnMode;
        [ConditionalHide("spawnMode", true)]
        public List<GameObject> enemiesToSpawn;
        [ConditionalHide("spawnMode", true)]
        public List<Transform> spawnPoints;
        [ConditionalHide("spawnMode", true)]
        public SpawnSystem spawnSystem;
        [ConditionalHide(true, false, "spawnMode", "spawnSystem")]
        [Min(0)]
        public int numberOfRounds;
        [ConditionalHide(true, false, "spawnSystem", "spawnMode")]
        [Min(1)]
        public int enemiesPerRound = 1;
        [ConditionalHide(true, false, "spawnSystem", "spawnMode")]
        public bool useTime;
        [ConditionalHide(true, false, "useTime", "spawnMode")]    
        [Min(1)]
        public float timeBetweenSpawnsSeconds = 1.0f;
        [ConditionalHide(true, false, "spawnSystem", "spawnMode")]
        public bool enableGUI;
        private List<GameObject> enemiesSpawned = new List<GameObject>();

        public static EnemyManager Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            } 
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!spawnMode) return;
            
            if (enemiesToSpawn.Count == 0)
            {
                Debug.LogError("No enemies to spawn");
                return;
            }

            if (spawnPoints.Count == 0)
            {
                Debug.LogError("No spawn points");
                return;
            }

            if (spawnSystem == SpawnSystem.RoundBased)
            {
                StartCoroutine(nameof(RoundBased));
                return;
            }

            StartCoroutine(nameof(SpawnEnemies));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SpawnEnemy()
        {
            
        }
        
        private IEnumerator SpawnEnemies()
        {
            enemiesSpawned.Add(Instantiate(enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)],
                spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity));
            yield return new WaitForSeconds(1.0f);
        }

        private IEnumerator RoundBased()
        {
            if (numberOfRounds > 0)
            {
                for (int i = 0; i < numberOfRounds; i++)
                {
                    for (int j = 0; j < enemiesPerRound; j++)
                    {
                        enemiesSpawned.Add(Instantiate(enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)],
                            spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity));
                    }
                    if (useTime)
                    {
                        yield return new WaitForSeconds(timeBetweenSpawnsSeconds);
                    }
                }
            }
            else
            {
                while (true)
                {
                    for (int i = 0; i < enemiesPerRound; i++)
                    {
                         enemiesSpawned.Add(Instantiate(enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)],
                             spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity));                       
                    }
                    yield return new WaitForSeconds(timeBetweenSpawnsSeconds);
                } 
            }
        }
        
        
    }
}