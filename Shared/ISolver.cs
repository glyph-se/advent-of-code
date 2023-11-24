namespace Shared;

public interface ISolver
{
	Task<string> PartOne(string input);

	Task<string> PartTwo(string input);
}
