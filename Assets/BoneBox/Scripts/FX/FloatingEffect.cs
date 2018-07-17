using UnityEngine;

namespace BoneBox.FX
{
	public class FloatingEffect : MonoBehaviour
	{
		private float m_Timer;

		[SerializeField] private float m_Speed;
		[SerializeField] private float m_Range;
		[SerializeField] private bool m_StartRandom = true;

		private void Awake()
		{
			if (m_StartRandom)
			{
				m_Timer = Random.value * (Mathf.PI);
			}
		}

		private void Update()
		{
			m_Timer += Time.deltaTime;

			Vector3 pos = transform.position;
			pos.y += Mathf.Sin(m_Timer * m_Speed) * m_Range * Time.deltaTime;
			transform.position = pos;
		}
	}
}