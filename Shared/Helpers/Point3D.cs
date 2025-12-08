using System.Diagnostics;

namespace Shared.Helpers;

[DebuggerDisplay("x={x},y={y},z={z}")]
public class Point3D
{
	public Point3D()
	{

	}

	public Point3D(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public int x { get; set; }
	public int y { get; set; }
	public int z { get; set; }

	public override bool Equals(object? obj)
	{
		return obj is Point3D point &&
				 x == point.x &&
				 y == point.y &&
				 z == point.z;

	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z);
	}
}
