using System;

namespace AK.Wwise
{
	[Serializable]
	public class AcousticTexture : BaseType
	{
		public override WwiseObjectType WwiseObjectType
		{
			get
			{
				return WwiseObjectType.AcousticTexture;
			}
		}
	}
}
