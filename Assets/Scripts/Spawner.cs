using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Flocking
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField] private float _lightIntensityAmp = 1f;
		[SerializeField] private bool _randomizeLightColor = false;
		[SerializeField] private float _movementSpeed = 1f;
		[Header("Spawning"), SerializeField] private GameObject _prefab;
		[SerializeField] private int _maxSpawnDelay = 120;
		[SerializeField] private float _boidColorResetTime;

		private GameManager _gameManager;

		private Light _light;

		private float _spawnRateModulus;
		private float _randomNumber;
		private float _boidColorResetTimer;

		private Vector3 _currentDirection;
		private Vector3 _newDirection;

		private GameObject _container;
		private GameObject _leaderContainer;

		private List<Boid> _boidList;

		private Color _lightColor;
		private Color _boidColor;

		public List<Boid> BoidList { get { return _boidList; } }
		public GameObject Container { get { return _container; } }
		public GameObject LeaderContainer { get { return _leaderContainer; } }

		private void Awake()
		{
			_boidList = new List<Boid>();
			_container = new GameObject("Group");
			_leaderContainer = new GameObject("Leader Group");

			_randomNumber = Random.value * 100;
			_newDirection = Random.insideUnitSphere;
			_currentDirection = _newDirection;
			_spawnRateModulus = 1;
			_boidColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);

			_light = GetComponent<Light>();
		}

		private void AssignRandomLeader()
		{
			Boid leader = _boidList[Random.Range(0, _boidList.Count)];
			leader.IsLeader = true;
			leader.name += "Leader";
			leader.transform.SetParent(_leaderContainer.transform);
		}

		private void Start()
		{
			_gameManager = GameManager.Instance;

			if (_gameManager.AssignRandomLeader)
			{
				AssignRandomLeader();
			}
		}

		public Boid Spawn(Vector3 pos)
		{
			Boid boid = Instantiate(_prefab, pos, Quaternion.identity).GetComponent<Boid>();
			_boidList.Add(boid);
			boid.Init(this, _boidList.Count, _boidColor);
			boid.transform.SetParent(boid.IsLeader ? _leaderContainer.transform : _container.transform);

			return boid;
		}

		/// <summary>
		/// Randomizes our direction and makes sure we move smoothly.
		/// </summary>
		private void HandleMovement()
		{
			if (Random.value < .1f) {
				_newDirection = Random.insideUnitSphere;
			}

			_currentDirection = Vector3.Slerp(_currentDirection, _newDirection, Time.deltaTime);

			transform.Translate(_currentDirection * _movementSpeed * Time.deltaTime);
		}

		#region Light Effects

		private void RandomizeLightIntensity()
		{
			_light.intensity = Mathf.PerlinNoise(_randomNumber + Time.time, _randomNumber + 1 + Time.time) * _lightIntensityAmp;
		}

		private void RandomizeLightColor()
		{
			_lightColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
			_light.color = Color.Lerp(_light.color, _lightColor, Time.deltaTime);
		}

		#endregion Light Effects

		private void Update()
		{
			_boidColorResetTimer += Time.deltaTime;

			if (_boidColorResetTimer >= _boidColorResetTime)
			{
				_boidColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f); // Random hue.
				_boidColorResetTimer = 0f;
			}

			if (_gameManager.Updates % (int) _spawnRateModulus == 0f)
			{
				Spawn(transform.position);

				if (_spawnRateModulus < _maxSpawnDelay)
				{
					_spawnRateModulus *= 1.1f;
				}

				if (Random.value < .1f)
				{
					_spawnRateModulus = 20;
				}
			}

			RandomizeLightIntensity();

			if (_randomizeLightColor)
			{
				RandomizeLightColor();
			}

			HandleMovement();
		}
	}
}