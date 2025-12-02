using System.Numerics;
using System.Runtime.CompilerServices;

namespace Shared.Helpers;

public class MathHelpers
{
	/// <remarks>From https://github.com/viceroypenguin/adventofcode/blob/master/AdventOfCode.Common/Extensions/NumberExtensions.cs</remarks>
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static long gcd(long a, long b)
	{
		while (b != 0) b = a % (a = b);
		return a;
	}

	/// <remarks>From https://github.com/viceroypenguin/adventofcode/blob/master/AdventOfCode.Common/Extensions/NumberExtensions.cs</remarks>
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static long lcm(long a, long b)
	{
		return a * b / gcd(a, b);
	}

	/// <summary>
	/// The '%' in C# is the <c>remainer</c>, not the <c>modulo</c>
	/// </summary>
	/// <remarks>From https://stackoverflow.com/a/51018529</remarks>
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static long mod(long x, long m)
	{
		long r = x % m;
		return r < 0 ? r + m : r;
	}

	/// <remarks>From https://github.com/encse/adventofcode/blob/master/2020/Day13/Solution.cs</remarks>
	public static long ChineseRemainderTheorem((long mod, long a)[] items)
	{
		var prod = items.Aggregate(1L, (acc, item) => acc * item.mod);
		var sum = items.Select((item, i) => {
			var p = prod / item.mod;
			return item.a * ModInv(p, item.mod) * p;
		}).Sum();

		return sum % prod;
	}

	public static long ModInv(long a, long m) => (long)BigInteger.ModPow(a, m - 2, m);
}
