using System.Diagnostics;

namespace Shared.Helpers
{
	[DebuggerDisplay("x={x},y={y}")]
	public class Point
	{
		public Point()
		{

		}

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int x { get; set; }
		public int y { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is Point point &&
					 x == point.x &&
					 y == point.y;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}
	}
}
