using UnityEngine;

namespace BoneBox.UI.FX.Base
{
	public class GUIColorChangeBase : GUIFXBase
	{
		[SerializeField] protected Color m_TargetColor;

		private Color m_OriginalColor;

		public Color OriginalColor { get { return m_OriginalColor; } }
		public Color TargetColor { get { return m_TargetColor; } set { m_TargetColor = value; } }
		
		protected override void Awake()
		{
			base.Awake();
			
			m_OriginalColor = m_Renderer.GetColor();
		}

		/// <summary>
		/// Sets the renderer color to the original color.
		/// </summary>
		public void ResetColor()
		{
			m_Renderer.SetColor(m_OriginalColor);
		}
	}
}
