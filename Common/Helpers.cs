namespace AdventOfCode.Common
{
    internal static class Helpers
    {
        /// <summary>
        /// Generates the Cartesian Product of the two given <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <remarks>From https://dev.azure.com/aliceif/AdventOfCodes/_git/AdventOfCodes?path=/advent2022/day04cs/Helpers.cs&version=GBmain&line=95&lineEnd=96&lineStartColumn=1&lineEndColumn=1&lineStyle=plain&_a=contents</remarks>
        public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(this IEnumerable<T1> one, IEnumerable<T2> two)
        {
            return one.SelectMany(i => two.Select(j => (i, j)));
        }

        /// <summary>
        /// Swaps the rows and columns of a nested sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>A sequence whose rows and columns are swapped.</returns>
        /// <remarks>From https://stackoverflow.com/a/5039692</remarks>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(
                 this IEnumerable<IEnumerable<T>> source)
        {
            return from row in source
                   from col in row.Select(
                       (x, i) => new KeyValuePair<int, T>(i, x))
                   group col.Value by col.Key into c
                   select c as IEnumerable<T>;
        }
    }
}
