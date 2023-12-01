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
    }
}