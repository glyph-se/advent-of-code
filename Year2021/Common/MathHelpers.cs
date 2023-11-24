using System.Runtime.CompilerServices;

namespace Year2021.Common;

internal class MathHelpers
{
    /// <remarks>From https://github.com/viceroypenguin/adventofcode/blob/master/AdventOfCode.Common/Helpers.cs</remarks>
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static long gcd(long a, long b)
    {
        while (b != 0) b = a % (a = b);
        return a;
    }

    /// <remarks>From https://github.com/viceroypenguin/adventofcode/blob/master/AdventOfCode.Common/Helpers.cs</remarks>
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static long lcm(long a, long b)
    {
        return a * b / gcd(a, b);
    }
}
