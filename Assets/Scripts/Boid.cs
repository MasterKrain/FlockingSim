using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Flocking
{
	using Utils;

	public class Boid : MonoBehaviour
	{
		[SerializeField] private Light _light = null;
		[SerializeField] private bool _randomizeLightColor = false;
		[SerializeField] private bool _isLeader = false;
		[SerializeField] private float _ageMax = 60;
		[SerializeField] private float _movementSpeed = 4f;
		[SerializeField] private float _influenceRange = 10f;
		[SerializeField] private float _attractionEdge = .6f;
		[SerializeField] private float _alignmentEdge = .3f;
		[SerializeField] private int _groupMinimum = 20;
		[SerializeField, Range(0, 1)] private float _randomMovingness = .85f;

		private GameManager _gameManager;
		private Spawner _spawner;
		private TrailRenderer _trailRenderer;

		private bool _initialised;
		private int _id;
		private int _amountOfOthers;
		private float _age;
		private float _currentMovementSpeed;
		private float _startRange;
		private Vector3 _currentDirection;
		private Vector3 _newDirection;
		private Color _lightColor;

		public int ID { get { return _id; } }
		public bool IsLeader { get { return _isLeader; } set { _isLeader = value; } }
		public Vector3 CurrentDirection { get { return _currentDirection; } }

		private Vector3 CalculateAdditionalDirection()
		{
			Vector3 newDirection = Vector3.zero;

			Collider[] hitColliders = Physics.OverlapSphere(transform.position, _influenceRange);

			Vector3 groupCenter = MathUtils.FindAveragePosition(hitColliders);

			for (int i = 0; i < hitColliders.Length; ++i)
			{
				Boid other = hitColliders[i].transform.parent.GetComponent<Boid>();
				float dist = Vector3.Distance(transform.position, other.transform.position);
				Vector3 dirToOther = other.transform.position - transform.position;

				// Polity
				if (GameManager.Instance.Dictatorship)
				{
					if (_isLeader && other.IsLeader && dist < _influenceRange / 2)
					{
						SetLeader(false);
						other.SetLeader(true);
					}
				}

				#region Flocking Rules

				// attraction
				if (dist > (_influenceRange * _attractionEdge))
				{
					newDirection += dirToOther * ((other.IsLeader) ? 10f : .01f);
				}

				// alignment
				if (dist < (_influenceRange * _attractionEdge) && dist > (_influenceRange * _alignmentEdge))
				{
					newDirection += other.CurrentDirection * ((other.IsLeader) ? 1f : .01f);
				}

				// repulsion
				if (dist < (_influenceRange * _alignmentEdge))
				{
					newDirection -= dirToOther * .01f;
				}

				#endregion Flocking Rules

				// Whirlpool Effect (WIP)
				if (hitColliders.Length > _groupMinimum && GameManager.Instance.DrawGroupCenters)
				{
					// add positive y angle perpendicular to direction to group center to directionNew...

					Debug.DrawRay(transform.position, groupCenter - transform.position, _lightColor);
				}
			}

			return newDirection;
		}

		public void SetLeader(bool leader)
		{
			if (!_initialised)
				return;

			_isLeader = leader;
			if (leader)
			{
				transform.SetParent(_spawner.LeaderContainer.transform);
			}
			else
			{
				transform.SetParent(_spawner.Container.transform);
			}
		}

		void Awake()
		{
			_currentMovementSpeed = _movementSpeed * 2f;
			_startRange = _light.range;
			_light.range = .1f;

			_age = .0f;

			if (_randomizeLightColor)
			{
				_lightColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
			}

			_newDirection = Random.insideUnitSphere;
			_currentDirection = _newDirection;

			_trailRenderer = GetComponent<TrailRenderer>();

			_trailRenderer.startColor = _lightColor;
			_trailRenderer.endColor = _lightColor;
		}

		void Start()
		{
			_gameManager = GameManager.Instance;
		}

		public void Init(Spawner spawner, int id, Color color)
		{
			_spawner = spawner;
			_id = id;
			_lightColor = color;
			_initialised = true;

			SetLeader(Random.value < .05f);
		}

		private void HandleMovement()
		{
			Vector3 additionalDirection = CalculateAdditionalDirection();
			_newDirection = (_newDirection + additionalDirection).normalized;

			if (Random.value < _randomMovingness)
			{
				_newDirection += Random.insideUnitSphere;
			}

			_currentDirection = Vector3.Lerp(_currentDirection, _newDirection.normalized, Time.deltaTime);

			transform.Translate(_currentDirection * _currentMovementSpeed * Time.deltaTime);
		}

		private void Update()
		{
			_age += Time.deltaTime;

			if (_currentMovementSpeed > _movementSpeed)
			{
				_currentMovementSpeed -= .1f * Time.deltaTime;
			}

			_light.color = _lightColor;

			if (_age >= _ageMax)
			{
				if (transform.localScale.magnitude > .0f)
				{
					Vector3 scale = transform.localScale;
					scale.x -= .001f;
					scale.y -= .001f;
					scale.z -= .001f;
					transform.localScale = scale;
				}

				_trailRenderer.startWidth = transform.localScale.x;
				_light.range -= .002f;

				if (_light.range <= .01f)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				if (_light.range < _startRange)
				{
					_light.range += .01f;
				}
			}

			HandleMovement();
		}
	}
}