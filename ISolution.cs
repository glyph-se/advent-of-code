namespace AdventOfCode
{
    internal interface ISolution
    {
        Task<string> PartOne(string input);

        Task<string> PartTwo(string input);
    }
}
