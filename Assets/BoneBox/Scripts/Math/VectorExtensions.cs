using UnityEngine;

namespace BoneBox.Maths
{
	public static class VectorExtensions
	{
		public static Vector3 MultipliedBy(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Vector2 MultipliedBy(this Vector2 a, Vector2 b)
		{
			return MultipliedBy((Vector3) a, (Vector3) b);
		}

		public static Vector3 NormalizedDifference(this Vector3 a, Vector3 b)
		{
			Vector3 c = a - b;
			float length = c.magnitude;

			if (length > 0.0f)
			{
				c.x /= length;
				c.y /= length;
				c.z /= length;
			}

			return c;
		}

		public static Vector2 NormalizedDifference(this Vector2 a, Vector2 b)
		{
			return NormalizedDifference((Vector3) a, (Vector3) b);
		}
	}
}
