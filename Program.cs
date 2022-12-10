namespace AdventOfCode
{
    internal class Program
    {
        static readonly int YEAR = 2022;
        static readonly int DAY = 10;

        public static async Task Main(string[] args)
        {
            PrintHeader(YEAR, DAY);

            ISolver? instance = GetSolverInstance(YEAR, DAY);

            if (instance == null)
            {
                PrintError("Solver not found!");
                return;
            }

            string? inputs = await InputService.GetInput(YEAR, DAY);

            if (inputs == null)
            {
                PrintError("No input found!");
                return;
            }

            Console.WriteLine("Running part one...");
            string outputOne = await instance.PartOne(inputs);
            PrintOutput(outputOne);

            Console.WriteLine("Running part two...");
            string outputTwo = await instance.PartTwo(inputs);
            PrintOutput(outputTwo);

            PrintFooter();
        }

        private static void PrintFooter()
        {
            Console.Write("--------------------------------------------------------------------------------\n");
        }

        private static void PrintOutput(string output)
        {
            Console.Write("Output is: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(output);
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static ISolver? GetSolverInstance(int year, int day)
        {
            Type? solverType = Type.GetType($"AdventOfCode.Year{year}.Day{day:D2}.Solver");

            if (solverType == null)
            {
                PrintError("Check constants (type not found)!");
                return null;
            }

            ISolver? solverInstance = Activator.CreateInstance(solverType) as ISolver;

            if (solverInstance == null)
            {
                PrintError("Check constants (constructor failed)!");
                return null;
            }

            return solverInstance;
        }

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void PrintHeader(int year, int day)
        {
            Console.Write("--------------------------------------------------------------------------------\n");
            Console.Write("|                              Advent of Code                                   |\n");
            Console.Write("--------------------------------------------------------------------------------\n");
            Console.Write("Year: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(year);
            Console.ResetColor();
            Console.Write(" ");
            Console.Write("Day: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(day);
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}