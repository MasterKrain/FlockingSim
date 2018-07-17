using UnityEngine;

namespace BoneBox.UI.FX.Base
{
	/// <summary>
	/// Base class for all the visual UI Component effects.
	/// </summary>
	[RequireComponent(typeof(CanvasRenderer))]
	public class GUIFXBase : MonoBehaviour
	{
		protected CanvasRenderer m_Renderer;

		protected virtual void Awake()
		{
			m_Renderer = GetComponent<CanvasRenderer>();
		}
	}
}
