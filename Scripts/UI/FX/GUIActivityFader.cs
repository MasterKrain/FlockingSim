using UnityEngine;

namespace BoneBox.UI.FX
{
	/// <summary>
	/// WIP.
	/// </summary>
	public class GUIActivityFader : GUIColorFader
	{
		[SerializeField] private bool m_FadeOnEnable = true;
		[SerializeField] private bool m_FadeOnDisable = true;

		private void OnEnable()
		{
			Awake();

			if (m_FadeOnEnable)
			{
				Fade(!m_IsInverted);
			}
		}

		private void OnDisable()
		{
			if (m_FadeOnDisable)
			{
				Fade(m_IsInverted);
			}
		}
	}
}
