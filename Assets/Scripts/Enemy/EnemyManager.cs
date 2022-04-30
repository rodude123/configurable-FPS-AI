using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using Assets.Scripts.utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemy
{

	[Serializable]
	public enum SpawnSystem
	{
		Continuous,
		RoundBased
	}

	[AddComponentMenu("FPS AI System/Enemy Manager")]
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
		[ConditionalHide(true, false, "spawnMode", "spawnSystem")]
		[Min(1)]
		public int enemiesPerRound = 1;
		[ConditionalHide(true, false, "spawnMode", "spawnSystem")]
		[Min(1)]
		public int enemiesMaintainedPerRound = 1;
		[ConditionalHide(true, false, "spawnMode", "spawnSystem")]
		public bool increaseEnemiesPerRound;
		[ConditionalHide(true, false, "spawnMode", "spawnSystem", "increaseEnemiesPerRound")]
		[Min(1)]
		public int numberToIncreasePerRound = 1;
		public bool useTime;
		[ConditionalHide(true, false, "spawnMode", "useTime", "spawnSystem")]
		[Min(1)]
		public float timeBetweenRoundsSeconds;
		[ConditionalHide(true, false, "spawnMode", "spawnSystem")]
		public bool enableUI;
		[ConditionalHide(true, false, "spawnMode", "spawnSystem", "enableUI")]
		public Canvas canvasUI;
		[ConditionalHide(true, false, "spawnMode", "spawnSystem", "enableUI")]
		public Color32 textColour;

		[DoNotSerialize]
		public int EnemiesKilled
		{
			get;
			set;
		}

		[DoNotSerialize]
		public readonly Queue<GameObject> enemiesSpawned = new Queue<GameObject>();

		private GameObject _counterGb;
		private int _currRound = 1;
		private int _currEnemiesSpawned = 0;
		private float _currTime;

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
				return;
			}
			Destroy(gameObject);
		}

		// Start is called before the first frame update

		private void Start()
		{
			if (!spawnMode)
			{
				return;
			}
			
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

			if (enemiesMaintainedPerRound > enemiesPerRound)
			{
				Debug.LogError("Enemies maintained per round cannot be greater than enemies per round");
				return;
			}

			for (var i = 0; i < enemiesMaintainedPerRound; i++)
			{
				var enemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)],
					spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity);
				enemy.SetActive(false);
				enemiesSpawned.Enqueue(enemy);
			}

			if (spawnSystem == SpawnSystem.RoundBased && enableUI)
			{
				StartCoroutine(nameof(RoundCounterUI));
			}
		}

		// Update is called once per frame

		private void Update()
		{
			if (!spawnMode)
			{
				return;
			}

			if (spawnSystem == SpawnSystem.RoundBased)
			{
				if (_currRound >= numberOfRounds)
				{
					return;
				}

				if (EnemiesKilled == enemiesPerRound)
				{
					if (useTime)
					{
						if (_currTime == 0)
						{
							_currTime = Time.time;
							return;
						}

						if (Time.time - _currTime >= timeBetweenRoundsSeconds)
						{
							_currTime = 0;
						}
					}

					_currRound++;
					if (increaseEnemiesPerRound)
					{
						enemiesPerRound += numberToIncreasePerRound;
					}
					_currEnemiesSpawned = -1;
					EnemiesKilled = -1;

					if (enableUI)
					{
						_counterGb.GetComponent<TextMeshProUGUI>().text = "Round: " + _currRound;
					}
					return;
				}

				if (_currEnemiesSpawned < enemiesPerRound && enemiesSpawned.Count > 0)
				{
					_currEnemiesSpawned++;
					SpawnEnemy();
				}
				return;
			}

			if (enemiesSpawned.Count > 0)
			{
				SpawnEnemy();
			}
		}

		private void SpawnEnemy()
		{
			var enemy = enemiesSpawned.Dequeue();
			enemy.GetComponent<Enemy>().Enable();
		}

		private IEnumerator RoundCounterUI()
		{
			var countInGb = new GameObject();
			var countIn = CreateTextElement(countInGb, new Vector2(Screen.width / 2, Screen.height * 0.875f), 72, "5");
			countInGb.transform.SetParent(canvasUI.transform);
			Time.timeScale = 0;

			for (var i = 3; i > 0; i--)
			{
				countIn.text = i.ToString();
				yield return new WaitForSecondsRealtime(1f);
			}

			yield return new WaitForSecondsRealtime(1f);
			countIn.text = "GO!";
			yield return new WaitForSecondsRealtime(1f);

			yield return new WaitForSecondsRealtime(1f);
			Time.timeScale = 1;
			Destroy(countInGb);
			_counterGb = new GameObject();
			CreateTextElement(_counterGb,
				new Vector2(Screen.width * 0.125f, Screen.height * 0.875f), 32, "Round: 1");
			_counterGb.transform.SetParent(canvasUI.transform);
		}

		private TextMeshProUGUI CreateTextElement(GameObject gameObj, Vector2 pos, int fontSize, string textStr)
		{
			var textMeshElem = gameObj.AddComponent<TextMeshProUGUI>();
			textMeshElem.text = textStr;
			textMeshElem.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as TMP_FontAsset;
			textMeshElem.fontSize = fontSize;
			textMeshElem.color = textColour;
			textMeshElem.alignment = TextAlignmentOptions.Center;
			var rt = gameObj.GetComponent<RectTransform>();
			rt.localPosition = pos;
			return textMeshElem;
		}
	}
}