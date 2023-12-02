using Shared;
using Shared.Helpers;

namespace Year2021.Day03;

public class Solver : ISolver
{
    public async Task<string> PartOne(string input)
    {
        await Task.Yield();

        string first = StringParsing.AsLines(input).First();

        string gammaRate = "";
        string epsilonRate = "";

        for (int i = 0; i < first.Length; i++)
        {
            int bit0 = 0;
            int bit1 = 0;

            foreach (var bits in StringParsing.AsLines(input))
            {
                if (bits[i] == '0')
                {
                    bit0++;
                }
                if (bits[i] == '1')
                {
                    bit1++;
                }
            }

            if (bit0 > bit1)
            {
                gammaRate += "0";
                epsilonRate += "1";
            }
            if (bit1 > bit0)
            {
                gammaRate += "1";
                epsilonRate += "0";
            }
        }

        var result = Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2);

        return result.ToString();
    }

    public async Task<string> PartTwo(string input)
    {
        await Task.Yield();

        IEnumerable<string> lines = StringParsing.AsLines(input);
        string first = lines.First();

        string oxygen = "";
        string co2 = "";

        for (int i = 0; i < first.Length; i++)
        {
            int bit0 = 0;
            int bit1 = 0;
            CountBitsAtPos(lines, i, ref bit0, ref bit1);

            if (bit0 > bit1)
            {
                lines = lines.Where(l => l[i] == '0').ToList();
            }
            else
            {
                lines = lines.Where(l => l[i] == '1').ToList();
            }

            if (lines.Count() == 1)
            {
                oxygen = lines.First();
            }
        }

        lines = StringParsing.AsLines(input);

        for (int i = 0; i < first.Length; i++)
        {
            int bit0 = 0;
            int bit1 = 0;
            CountBitsAtPos(lines, i, ref bit0, ref bit1);

            if (bit1 < bit0)
            {
                lines = lines.Where(l => l[i] == '1').ToList();
            }
            else
            {
                lines = lines.Where(l => l[i] == '0').ToList();
            }

            if (lines.Count() == 1)
            {
                co2 = lines.First();
            }
        }

        var result = Convert.ToInt32(co2, 2) * Convert.ToInt32(oxygen, 2);

        return result.ToString();
    }

    private static void CountBitsAtPos(IEnumerable<string> lines, int i, ref int bit0, ref int bit1)
    {
        foreach (var bits in lines)
        {
            if (bits[i] == '0')
            {
                bit0++;
            }
            if (bits[i] == '1')
            {
                bit1++;
            }
        }
    }
}
