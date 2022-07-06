using SharpDX;

namespace Run8Tools.InternalClasses
{
	internal sealed class MRL
	{
		internal MRL(int I, TimeSpan L, Matrix P)
		{
			this.Bone = I;
			this.Time = L;
			this.Transform = P;
		}

		private MRL()
		{
		}

		internal int Bone { get; private set; }

		internal TimeSpan Time { get; private set; }

		internal Matrix Transform { get; set; }

		//// Token: 0x0400219B RID: 8603
		//[CompilerGenerated]
		//private int I;

		//// Token: 0x0400219C RID: 8604
		//[CompilerGenerated]
		//private TimeSpan L;

		//// Token: 0x0400219D RID: 8605
		//[CompilerGenerated]
		//private Matrix P;
	}
}
