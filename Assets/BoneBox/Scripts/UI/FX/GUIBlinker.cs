using UnityEngine;

namespace BoneBox.UI.FX
{
	using Base;

	public class GUIBlinker : GUIColorChangeBase
	{
		[SerializeField] private bool m_IsEnabled = true;

		[SerializeField, Tooltip("How fast to blink in seconds.")] private float m_Speed = 1.0f;

		private float m_Timer;

		public bool IsEnabled { get { return m_IsEnabled; } set { if ((m_IsEnabled = value) == false) ResetColor(); } }
		public float Speed { get { return m_Speed; } set { m_Speed = value; } }
		public float Timer { get { return m_Timer; } }

		protected virtual void Update()
		{
			if (m_IsEnabled)
			{
				m_Timer += Time.deltaTime;

				if (m_Timer >= m_Speed)
				{
					m_Timer = 0.0f;

					Color currentColor = m_Renderer.GetColor();
					MemoryUtils.Switch(ref currentColor, OriginalColor, m_TargetColor);
					m_Renderer.SetColor(currentColor);
				}
			}
		}
	}
}
