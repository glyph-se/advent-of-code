using System.Diagnostics;
using AdventOfCode.Common;

namespace AdventOfCode.Year2022.Day20
{
    internal class Solver : ISolver
    {
        public async Task<string> PartOne(string input)
        {
            await Task.Yield();

            List<EncryptionNumber> numbersList = new();

            int[] original = input.AsInts().ToArray();
            EncryptionNumber[] oldList = new EncryptionNumber[original.Length];

            for (int pos = 0; pos < original.Length; pos++)
            {
                var ne = new EncryptionNumber()
                {
                    number = original[pos]
                };
                numbersList.Add(ne);
                oldList[pos] = ne;
            }

            Queue<EncryptionNumber> numbersToMove = new Queue<EncryptionNumber>(oldList);

            while (numbersToMove.Any())
            {
                var ne = numbersToMove.Dequeue();

                int index = numbersList.IndexOf(ne);

                int newIndex = (int)((index + ne.number) % (numbersList.Count - 1));

                while (newIndex <= 0)
                {
                    newIndex += (numbersList.Count - 1);
                }

                numbersList.Remove(ne);
                numbersList.Insert(newIndex, ne);
            }

            long result = 0;

            EncryptionNumber zero = numbersList
                .Single(ne => ne.number == 0);

            int zeroPos = numbersList.IndexOf(zero);

            result += numbersList[(zeroPos + 1000) % (numbersList.Count)].number;
            result += numbersList[(zeroPos + 2000) % (numbersList.Count)].number;
            result += numbersList[(zeroPos + 3000) % (numbersList.Count)].number;

            return result.ToString();
        }

        [DebuggerDisplay("{number}")]
        public class EncryptionNumber
        {
            public long number;
        }

        public async Task<string> PartTwo(string input)
        {
            const long DecryptionKey = 811589153L;

            await Task.Yield();

            List<EncryptionNumber> numbersList = new();

            int[] original = input.AsInts().ToArray();
            EncryptionNumber[] oldList = new EncryptionNumber[original.Length];

            for (int pos = 0; pos < original.Length; pos++)
            {
                var ne = new EncryptionNumber()
                {
                    number = original[pos] * DecryptionKey,
                };
                numbersList.Add(ne);
                oldList[pos] = ne;
            }

            Queue<EncryptionNumber> numbersToMove;

            for (int i = 1; i <= 10; i++)
            {
                numbersToMove = new Queue<EncryptionNumber>(oldList);

                while (numbersToMove.Any())
                {
                    var ne = numbersToMove.Dequeue();

                    int index = numbersList.IndexOf(ne);

                    int newIndex = (int)((index + ne.number) % (numbersList.Count - 1));

                    while (newIndex <= 0)
                    {
                        newIndex += (numbersList.Count - 1);
                    }

                    numbersList.Remove(ne);
                    numbersList.Insert(newIndex, ne);
                }
            }

            long result = 0;

            EncryptionNumber zero = numbersList
                .Single(ne => ne.number == 0);

            int zeroPos = numbersList.IndexOf(zero);

            result += numbersList[(zeroPos + 1000) % (numbersList.Count)].number;
            result += numbersList[(zeroPos + 2000) % (numbersList.Count)].number;
            result += numbersList[(zeroPos + 3000) % (numbersList.Count)].number;

            return result.ToString();
        }
    }
}
