using System.Reflection;

namespace AOC.Tests.Y2023
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day01
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase(@"1abc2
                    pqr3stu8vwx
                    a1b2c3d4e5f
                    treb7uchet", 142)]
        [TestCase(null, 54968)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            int result = lines.Sum(line =>
            {
                string? digits = new(line.Where(char.IsDigit).ToArray());
                digits = digits.Length switch
                {
                    1 => digits + digits,
                    > 2 => $"{digits[0]}{digits[^1]}",
                    _ => digits
                };
                return int.Parse(digits);
            });

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        [TestCase(@"two1nine
            eightwothree
            abcone2threexyz
            xtwone3four
            4nineeightseven2
            zoneight234
            7pqrstsixteen", 281)]
        [TestCase(null, 54094)] // The actual answer
        public void Part2(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            int result = lines.Sum(line =>
            {
                var digits = new List<int>();

                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        digits.Add(int.Parse(line[i].ToString()));
                    }
                    else
                    {
                        foreach (var spelledOutNumber in spelledOutDigits.Keys.OrderByDescending(s => s.Length))
                        {
                            if (i + spelledOutNumber.Length <= line.Length && line.Substring(i, spelledOutNumber.Length) == spelledOutNumber)
                            {
                                digits.Add(spelledOutDigits[spelledOutNumber]);
                                i += spelledOutNumber.Length - 2; // Skip the last character of the spelled-out number incase of overlap
                                break;
                            }
                        }
                    }
                }

                // Calculate the result for the current line
                int lineResult = digits.Count switch
                {
                    1 => Convert.ToInt32($"{digits[0]}{digits[0]}"),
                    > 1 => Convert.ToInt32($"{digits[0]}{digits[^1]}"),
                    _ => throw new Exception("No digits")
                };

                return lineResult;
            });

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }

        // Dictionary to map spelled-out digits to their numeric values
        private static readonly Dictionary<string, int> spelledOutDigits = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };
    }
}