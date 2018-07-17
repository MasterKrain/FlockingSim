using System;

using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

using BoneBox.Data.Storage;

namespace BoneBox.Debugging
{
	/// <summary>
	/// Logs enhanced messages to the Unity Console by specifying the sender and using rich text.
	/// </summary>
	public static class Logger
	{
		public enum TextColor
		{
			DEFAULT = 0,
			Grey,
			White,
			Black,
			Aqua,
			Cyan,
			Blue,
			Brown,
			DarkBlue,
			Fuchsia,
			Green,
			LightBlue,
			Lime,
			Magenta,
			Maroon,
			Navy,
			Olive,
			Orange,
			Purple,
			Red,
			Silver,
			Teal,
			Yellow
		}

		private static Cache m_Cache = new Cache();

		private static string GetFormattedMessage(string label, object message, TextColor messageColor = TextColor.DEFAULT, TextColor labelColor = TextColor.DEFAULT, bool recacheLabelColor = false)
		{
			string labelColorCacheKey = $"{label}_LabelColor";

			if (labelColor == TextColor.DEFAULT)
			{
				if (m_Cache.Contains(labelColorCacheKey))
				{
					object cachedLabelColor = m_Cache.Load(labelColorCacheKey);
					if (cachedLabelColor.GetType() == typeof(TextColor))
					{
						labelColor = (TextColor) cachedLabelColor;
					}
					else if (cachedLabelColor.GetType() == typeof(string))
					{
						if (Enum.IsDefined(typeof(TextColor), (string) cachedLabelColor))
						{
							labelColor = (TextColor) Enum.Parse(typeof(TextColor), (string) cachedLabelColor, true);
						}
					}
				}
				else
				{
					labelColor = CacheRandomLabelColor(label);
				}
			}
			else if (recacheLabelColor)
			{
				m_Cache.Save(labelColorCacheKey, labelColor);
			}

			return $"[<color={labelColor}>{label.Substring(label.LastIndexOf('.') + 1)}</color>] {(messageColor == TextColor.DEFAULT ? message : $"<color={messageColor}>{message}</color>")}";
		}

		#region Public Methods

		public static TextColor CacheRandomLabelColor(string label)
		{
			string labelColorCacheKey = $"{label}_LabelColor";
			TextColor randomColor = (TextColor) Random.Range(1, Enum.GetNames(typeof(TextColor)).Length);
			m_Cache.Save(labelColorCacheKey, randomColor);
			return randomColor;
		}

		public static void CacheRandomLabelColor(object sender)
		{
			CacheRandomLabelColor(sender.GetType().ToString());
		}

		public static void Log(string label, object message, TextColor messageColor = TextColor.DEFAULT, TextColor labelColor = TextColor.DEFAULT, bool recacheLabelColor = false)
		{
			Debug.Log(GetFormattedMessage(label, message, messageColor, labelColor, recacheLabelColor));
		}

		public static void Log(object sender, object message, TextColor messageColor = TextColor.DEFAULT, TextColor labelColor = TextColor.DEFAULT, bool recacheLabelColor = false)
		{
			Log(sender.GetType().ToString(), message, messageColor, labelColor, recacheLabelColor);
		}

		public static void LogError(string label, object message, TextColor labelColor = TextColor.DEFAULT, bool recacheLabelColor = false)
		{
			Debug.LogError(GetFormattedMessage(label, message, TextColor.Red, labelColor, recacheLabelColor));
		}

		public static void LogError(object sender, object message, TextColor labelColor = TextColor.DEFAULT, bool recacheLabelColor = false)
		{
			LogError(sender.GetType().ToString(), message, labelColor, recacheLabelColor);
		}

		public static void LogWarning(string label, object message, TextColor labelColor = TextColor.DEFAULT, bool recacheLabelColor = false)
		{
			Debug.LogWarning(GetFormattedMessage(label, message, TextColor.Orange, labelColor, recacheLabelColor));
		}

		public static void LogWarning(object sender, object message, TextColor labelColor = TextColor.DEFAULT, bool recacheLabelColor = false)
		{
			LogWarning(sender.GetType().ToString(), message, labelColor, recacheLabelColor);
		}

		#endregion
	}
}