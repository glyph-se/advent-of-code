using System.Diagnostics;

namespace Shared.Helpers
{
	[DebuggerDisplay("{c}, x={x},y={y}")]
	public class CharPoint : Point
	{
		public CharPoint()
		{

		}

		public CharPoint(char c, int x, int y)
		{
			this.x = x;
			this.y = y;
			this.c = c;
		}

		public char c { get; set; }
	}
}
