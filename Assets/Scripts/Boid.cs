using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Flocking.Management;
using Flocking.Util;

namespace Flocking
{
	public class Boid : MonoBehaviour
	{
		private Spawner m_Spawner;
		private TrailRenderer m_TrailRenderer;

		private int m_ID;
		private int m_AmountOfOthers;
		private float m_Age;
		private float m_CurrentMovementSpeed;
		private float m_StartRange;
		private Vector3 m_CurrentDirection;
		private Vector3 m_NewDirection;
		private Color m_LightColor;

		public int ID { get { return m_ID; } }
		public bool IsLeader { get { return m_IsLeader; } set { m_IsLeader = value; } }
		public Vector3 CurrentDirection { get { return m_CurrentDirection; } }

		[SerializeField] private bool m_IsLeader = false;
		[SerializeField] private float m_AgeMax = 60;
		[SerializeField] private float m_MovementSpeed;
		[SerializeField] private float m_InfluenceRange = 5f, m_AttractionEdge = .66f, m_AlignmentEdge = .33f;
		[SerializeField] private int m_GroupMinimum = 20;
		[SerializeField] private Light m_Light;
		[Range(0, 1)] [SerializeField] private float m_RandomMovingness = .3f;

		private void Awake()
		{
			m_TrailRenderer = GetComponent<TrailRenderer>();
		}

		private void Start()
		{
			m_CurrentMovementSpeed = m_MovementSpeed * 2f;

			SetLeader(Random.value < .05f);

			m_StartRange = m_Light.range;
			m_Light.range = .1f;

			m_Age = .0f;

			m_TrailRenderer.startColor = m_LightColor;
			m_TrailRenderer.endColor = m_LightColor;

			//m_LightColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);

			m_NewDirection = Random.insideUnitSphere;
			m_CurrentDirection = m_NewDirection;
		}

		public void Init(Spawner spawner, int id, Color color)
		{
			m_Spawner = spawner;
			m_ID = id;
			m_LightColor = color;
		}

		private void Update()
		{
			m_Age += Time.deltaTime;

			if (m_CurrentMovementSpeed > m_MovementSpeed) {
				m_CurrentMovementSpeed -= .1f * Time.deltaTime;
			}

			m_Light.color = m_LightColor;

			if (m_Age >= m_AgeMax) {
				if (transform.localScale.magnitude > .0f) {
					Vector3 scale = transform.localScale;
					scale.x -= .001f;
					scale.y -= .001f;
					scale.z -= .001f;
					transform.localScale = scale;
				}

				m_TrailRenderer.startWidth = transform.localScale.x;
				m_Light.range -= .002f;

				if (m_Light.range <= .01f) {
					Destroy(gameObject);
				}
			} else {
				if (m_Light.range < m_StartRange) {
					m_Light.range += .01f;
				}
			}

			HandleMovement();
		}

		public void SetLeader(bool leader)
		{
			m_IsLeader = leader;
			if (leader) {
				transform.SetParent(m_Spawner.LeaderContainer.transform);
			} else {
				transform.SetParent(m_Spawner.Container.transform);
			}
		}

		private void HandleMovement()
		{
			Vector3 additionalDirection = CalculateAdditionalDirection();
			m_NewDirection = (m_NewDirection + additionalDirection).normalized;

			if (Random.value < m_RandomMovingness) {
				m_NewDirection += Random.insideUnitSphere;
			}

			m_CurrentDirection = Vector3.Lerp(m_CurrentDirection, m_NewDirection.normalized, Time.deltaTime);

			transform.Translate(m_CurrentDirection * m_CurrentMovementSpeed * Time.deltaTime);
		}

		private Vector3 CalculateAdditionalDirection()
		{
			Vector3 newDirection = Vector3.zero;

			Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_InfluenceRange);

			Vector3 groupCenter = MathUtil.FindAveragePosition(hitColliders);

			for (int i = 0; i < hitColliders.Length; ++i) {
				Boid other = hitColliders[i].transform.parent.GetComponent<Boid>();
				float dist = Vector3.Distance(transform.position, other.transform.position);
				Vector3 dirToOther = other.transform.position - transform.position;

				// Polity
				if (GameManager.Instance.Dictatorship) {
					if (m_IsLeader && other.IsLeader && dist < m_InfluenceRange / 2) {
						SetLeader(false);
						other.SetLeader(true);
					}
				}

				#region Flocking Rules

				// attraction
				if (dist > (m_InfluenceRange * m_AttractionEdge)) {
					newDirection += dirToOther * ((other.IsLeader) ? 10f : .01f);
				}

				// alignment
				if (dist < (m_InfluenceRange * m_AttractionEdge) && dist > (m_InfluenceRange * m_AlignmentEdge)) {
					newDirection += other.CurrentDirection * ((other.IsLeader) ? 1f : .01f);
				}

				// repulsion
				if (dist < (m_InfluenceRange * m_AlignmentEdge)) {
					newDirection -= dirToOther * .01f;
				}

				#endregion Flocking Rules

				// Whirlpool Effect (WIP)
				if (hitColliders.Length > m_GroupMinimum && GameManager.Instance.DrawGroupCenters) {
					// add positive y angle perpendicular to direction to group center to directionNew...

					Debug.DrawRay(transform.position, groupCenter - transform.position, m_LightColor);
				}
			}

			return newDirection;
		}
	}
}