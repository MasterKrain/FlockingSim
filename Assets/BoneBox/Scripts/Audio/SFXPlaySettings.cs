using BoneBox.Data;

namespace BoneBox.Audio
{
	[System.Serializable]
	public struct SFXPlaySettings
	{
		//public AudioClip Clip { get; set; }
		public float Volume { get; set; }
		public MinMaxFloat PitchRange { get; set; }
		public bool Loop { get; set; }

		public SFXPlaySettings(/*AudioClip clip,*/ float volume, MinMaxFloat pitchRange, bool loop)
		{
			//Clip = clip;
			Volume = volume;
			PitchRange = pitchRange;
			Loop = loop;
		}
	}
}
