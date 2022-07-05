using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run8Tools
{
	internal sealed class URL
	{
		internal URL(TimeSpan I, List<MRL> L)
		{
			this.Duration = I;
			this.Keyframes = L;
		}

		private URL()
		{
		}

		internal TimeSpan Duration { get; private set; }

		internal List<MRL> Keyframes { get; private set; }

		public bool I;

		public bool L;

		//[CompilerGenerated]
		//private TimeSpan P;

		//// Token: 0x04002192 RID: 8594
		//[CompilerGenerated]
		//private List<MRL> F;
	}
}
