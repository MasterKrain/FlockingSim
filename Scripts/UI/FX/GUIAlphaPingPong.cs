using UnityEngine;

using BoneBox.UI.FX.Base;

namespace BoneBox.UI.FX
{
	public class GUIAlphaPingPong : GUIColorChangeBase
	{
		[SerializeField] private float m_Speed = 1.0f;

		private float m_Timer;

		private void Update()
		{
			m_Timer += Time.unscaledDeltaTime * m_Speed;

			m_Renderer.SetAlpha(Mathf.PingPong(m_Timer, 1.0f));
		}
	}
}
