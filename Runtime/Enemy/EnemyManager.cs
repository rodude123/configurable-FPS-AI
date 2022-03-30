using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using utilities;
using Random = UnityEngine.Random;

namespace Enemy
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
		public bool useTime;
		[ConditionalHide(true, false, "spawnMode", "useTime")]
		[Min(1)]
		public float timeBetweenSpawnsSeconds = 10.0f;
		[ConditionalHide(true, false, "spawnMode", "spawnSystem")]
		public bool enableUI;

		[ConditionalHide(true, false, "spawnMode", "spawnSystem", "enableUI")]
		public Canvas canvasUI;

		[ConditionalHide(true, false, "spawnMode", "spawnSystem", "enableUI")]
		public Color32 textColour;
		private GameObject _counterGb;
		private int _currRound;

		private readonly List<GameObject> _enemiesSpawned = new List<GameObject>();

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
		private void Start()
		{
			if (!spawnMode)
			{
				return;
			}

			if (spawnSystem == SpawnSystem.RoundBased)
			{
				StartCoroutine(nameof(RoundBased));
				if (enableUI)
				{
					StartCoroutine(nameof(RoundCounterUI));
				}
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

			StartCoroutine(nameof(SpawnEnemies));
		}

		// Update is called once per frame
		private void Update()
		{
			if (enableUI)
			{
				_counterGb.GetComponent<TextMeshProUGUI>().text = "Round: " + (_currRound + 1);
			}
		}

		public void SpawnEnemy()
		{
			if (_enemiesSpawned.Count > 5)
			{
				for (var i = 0; i < _enemiesSpawned.Count; i++)
				{
					if (_enemiesSpawned[i].activeInHierarchy == false)
					{
						_enemiesSpawned[i].SetActive(true);
						break;
					}
				}
			}

			_enemiesSpawned.Add(Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)],
				spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity));
		}

		private IEnumerator SpawnEnemies()
		{
			while (spawnSystem == SpawnSystem.Continuous)
			{
				for (var i = 0; i < 10; i++)
				{
					SpawnEnemy();
				}
				yield return new WaitForSeconds(timeBetweenSpawnsSeconds);
			}
		}

		private IEnumerator RoundBased()
		{
			if (numberOfRounds > 0)
			{
				for (var i = 0; i < numberOfRounds; i++)
				{
					_currRound = i;
					for (var j = 0; j < enemiesPerRound; j++)
					{
						SpawnEnemy();
					}

					if (useTime)
					{
						yield return new WaitForSeconds(timeBetweenSpawnsSeconds);
					}
				}
				yield break;
			}

			if (numberOfRounds == 0)
			{
				while (true)
				{
					for (var i = 0; i < enemiesPerRound; i++)
					{
						SpawnEnemy();
					}
					yield return new WaitForSeconds(timeBetweenSpawnsSeconds);
				}
			}

			

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
			var counterTextMesh = CreateTextElement(_counterGb,
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