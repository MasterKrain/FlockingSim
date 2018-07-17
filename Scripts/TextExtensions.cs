
namespace BoneBox
{
	public static class TextExtensions
	{
		public static char? FirstNonWhitespace(this string str)
		{
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				if (!char.IsWhiteSpace(str[i]))
				{
					return str[i];
				}
			}

			return null;
		}
	}
}
