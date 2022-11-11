namespace AdventOfCode
{
    internal interface ISolver
    {
        Task<string> PartOne(string input);

        Task<string> PartTwo(string input);
    }
}
