using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Flocking.Management;

namespace Flocking
{
	public class Spawner : MonoBehaviour
	{
		private GameManager m_GameManager;

		private float m_SpawnRateModulus;
		private float m_RandomNumber;
		private float m_BoidColorResetTimer;
		private Vector3 m_CurrentDirection;
		private Vector3 m_NewDirection;
		private GameObject m_Container;
		private GameObject m_LeaderContainer;
		private List<Boid> m_BoidList;
		private Light m_Light;
		private Color m_LightColor;
		private Color m_BoidColor;

		public List<Boid> BoidList { get { return m_BoidList; } }
		public GameObject Container { get { return m_Container; } }
		public GameObject LeaderContainer { get { return m_LeaderContainer; } }

		[SerializeField] private float m_LightIntensityAmp;
		[SerializeField] private float m_MovementSpeed;
		[Header("Spawning")] [SerializeField] private GameObject m_Prefab;
		[SerializeField] private int m_MaxSpawnDelay;
		[SerializeField] private float m_BoidColorResetTime;

		private void Awake()
		{
			m_BoidList = new List<Boid>();
			m_Container = new GameObject("Group");
			m_LeaderContainer = new GameObject("Leader Group");

			m_RandomNumber = Random.value * 100;
		}

		private void Start()
		{
			m_GameManager = FindObjectOfType<GameManager>();
			m_Light = GetComponent<Light>();

			//AssignRandomLeader();

			m_NewDirection = Random.insideUnitSphere;
			m_CurrentDirection = m_NewDirection;

			m_SpawnRateModulus = 1;

			m_BoidColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
		}

		private void Update()
		{
			m_BoidColorResetTimer += Time.deltaTime;

			if (m_BoidColorResetTimer >= m_BoidColorResetTime) {
				m_BoidColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
				m_BoidColorResetTimer = 0f;
			}

			if (m_GameManager.Updates % (int) m_SpawnRateModulus == 0f) {
				Spawn(transform.position);

				if (m_SpawnRateModulus < m_MaxSpawnDelay) {
					m_SpawnRateModulus *= 1.1f;
				}

				if (Random.value < .1f) {
					m_SpawnRateModulus = 20;
				}
			}

			RandomizeLightIntensity();
			//RandomizeLightColor();

			HandleMovement();
		}

		private void AssignRandomLeader()
		{
			Boid leader = m_BoidList[Random.Range(0, m_BoidList.Count)];
			leader.IsLeader = true;
			leader.name += "Leader";
		}

		private void HandleMovement()
		{
			if (Random.value < .1f) {
				m_NewDirection = Random.insideUnitSphere;
			}

			m_CurrentDirection = Vector3.Slerp(m_CurrentDirection, m_NewDirection, Time.deltaTime);

			transform.Translate(m_CurrentDirection * m_MovementSpeed * Time.deltaTime);
		}

		public Boid Spawn(Vector3 pos)
		{
			Boid boid = Instantiate(m_Prefab, pos, Quaternion.identity).GetComponent<Boid>();
			m_BoidList.Add(boid);
			boid.Init(this, m_BoidList.Count, m_BoidColor);

			return boid;
		}

		#region Light Effects

		private void RandomizeLightIntensity()
		{
			m_Light.intensity = Mathf.PerlinNoise(m_RandomNumber + Time.time, m_RandomNumber + 1 + Time.time) * m_LightIntensityAmp;
		}

		private void RandomizeLightColor()
		{
			m_LightColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
			m_Light.color = Color.Lerp(m_Light.color, m_LightColor, Time.deltaTime);
		}

		#endregion Light Effects
	}
}