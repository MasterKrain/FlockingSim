using System.Collections;

using UnityEngine;

namespace BoneBox.UI.FX
{
	using Base;

	/// <summary>
	/// Transition from the original color to the target color.
	/// </summary>
	public class GUIColorFader : GUIColorChangeBase
	{
		[SerializeField] protected bool m_AutoStart;
		[SerializeField] protected bool m_IsInverted;
		[SerializeField] protected bool m_IgnoreTimeScale = true;
		[SerializeField] protected float m_Speed = 1.0f;
		[SerializeField] protected AnimationCurve m_Curve;

		private Coroutine m_CurrentCrossFadeRoutine;
		
		public bool IsInverted { get { return m_IsInverted; } set { m_IsInverted = value; } }
		public bool IgnoreTimeScale { get { return m_IgnoreTimeScale; } set { m_IgnoreTimeScale = value; } }
		public float Speed { get { return m_Speed; } set { m_Speed = value; } }

		protected virtual void Start()
		{
			if (m_AutoStart)
			{
				Fade();
			}
		}

		/// <summary>
		/// WIP -_-
		/// </summary>
		/// <param name="targetColor"></param>
		/// <param name="speed"></param>
		/// <param name="ignoreTimeScale"></param>
		/// <returns></returns>
		private IEnumerator CrossFadeRoutine(Color targetColor, float speed, bool ignoreTimeScale)
		{
			float timer = 0.0f;
			Color originalColor = m_Renderer.GetColor();

			while (m_Renderer.GetColor() != targetColor)
			{
				timer += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * speed;
				m_Renderer.SetColor(Color.Lerp(originalColor, targetColor, m_Curve.Evaluate(timer)));
				yield return null;
			}

			m_CurrentCrossFadeRoutine = null;
		}

		public void CrossFade(Color targetColor, float speed, bool ignoreTimeScale)
		{
			if (m_CurrentCrossFadeRoutine == null)
			{
				m_CurrentCrossFadeRoutine = StartCoroutine(CrossFadeRoutine(targetColor, speed, ignoreTimeScale));
			}
		}

		public void Fade(bool inverted)
		{
			m_Renderer.SetColor(inverted ? m_TargetColor : OriginalColor);
			CrossFade(inverted ? OriginalColor : m_TargetColor, m_Speed, m_IgnoreTimeScale);
			//m_Graphic.CrossFadeColor(inverted ? OriginalColor : m_TargetColor, m_Duration, m_IgnoreTimeScale, true);
		}

		public void Fade()
		{
			Fade(m_IsInverted);
		}

		//private void Update()
		//{
		//	//print(name + " " + m_IsEnabled + " " + m_Loop + " " + m_Progress + " " + m_FadeType);
		//	if (m_IsEnabled /*&& (m_Loop || (!m_Loop && m_Progress < (m_FadeType == EFadeType.Both ? Mathf.PI  : MathUtils.HALF_PI)))*/)
		//	{
		//		//m_Graphic.CrossFadeColor(m_TargetColor, m_Duration, true, true);

		//		//m_Timer += Time.deltaTime;

		//		//m_Progress = m_Timer * m_Speed;
		//		//m_Value = Mathf.Abs((m_FadeType == EFadeType.TargetToOriginal) ? Mathf.Cos(m_Progress) : Mathf.Sin(m_Progress));

		//		//Color currentColor = m_Graphic.color;
		//		//currentColor = Color.Lerp(m_OriginalColor, m_TargetColor, m_Value);
		//		//m_Graphic.color = currentColor;

		//		//if (m_FadeType != EFadeType.Both && m_Progress >= MathUtils.HALF_PI)
		//		//{
		//		//	m_Timer = 0.0f;
		//		//}
		//	}
		//}
	}
}