using SharpDX;

namespace Run8Tools
{
	internal sealed class KRL
	{
		internal KRL(Dictionary<string, URL> AnimationClips, List<Matrix> BindPose, List<Matrix> InverseBindPose, List<int> SkeletonHierarchy, Dictionary<string, int> BoneIndecies)
		{
			this.AnimationClips = AnimationClips;
			this.BindPose = BindPose;
			this.InverseBindPose = InverseBindPose;
			this.SkeletonHierarchy = SkeletonHierarchy;
			this.BoneIndices = BoneIndecies;
		}

		private KRL()
		{
		}

		internal Dictionary<string, URL> AnimationClips { get; private set; }

		internal List<Matrix> BindPose { get; private set; }

		internal List<Matrix> InverseBindPose { get; private set; }

		internal List<int> SkeletonHierarchy { get; private set; }

		internal Dictionary<string, int> BoneIndices { get; private set; }

		//[CompilerGenerated]
		//private Dictionary<string, URL> I;

		//// Token: 0x0400219F RID: 8607
		//[CompilerGenerated]
		//private List<Matrix> L;

		//// Token: 0x040021A0 RID: 8608
		//[CompilerGenerated]
		//private List<Matrix> P;

		//// Token: 0x040021A1 RID: 8609
		//[CompilerGenerated]
		//private List<int> F;

		//// Token: 0x040021A2 RID: 8610
		//[CompilerGenerated]
		//private Dictionary<string, int> C;
	}
}
