using System.Reflection;

namespace AOC.Tests.Y2023
{
    public class Cube
    {
        public string Color { get; set; }
        public int Count { get; set; }
    }

    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day02
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase(@"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
                    Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
                    Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
                    Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
                    Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 8)]
        [TestCase(null, 2061)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string[] lines = input != null ? new[] { input } : realData;

            int result = GetPossibleGamesSum(lines, 12, 13, 14);

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        private static int GetPossibleGamesSum(string[] input, int redCount, int greenCount, int blueCount)
        {
            string[] games = string.Join("", input).Split(new[] { "Game " }, StringSplitOptions.RemoveEmptyEntries);

            int sum = 0;
            for (int i = 0; i < games.Length; i++) // Start the loop from index 1 to skip the first element
            {
                string game = games[i];

                // Exclude the game number from the game string
                string[] gameParts = game.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (gameParts.Length != 2)
                {
                    // Invalid game string, skip to the next one
                    continue;
                }

                string[] subsets = gameParts[1].Split(';', StringSplitOptions.RemoveEmptyEntries);
                HashSet<Cube> cubes = new HashSet<Cube>();

                foreach (string subset in subsets)
                {
                    string[] cubeParts = subset.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string cube in cubeParts)
                    {
                        string[] cubeInfo = cube.Trim().Split(' ');
                        string color = cubeInfo[1];
                        int count = int.Parse(cubeInfo[0]);

                        cubes.Add(new Cube { Color = color, Count = count });
                    }
                }

                bool isPossible = cubes.All(cube =>
                {
                    int count = cube.Color switch
                    {
                        "red" => redCount,
                        "green" => greenCount,
                        "blue" => blueCount,
                        _ => 0
                    };

                    return count >= cube.Count;
                });

                if (isPossible)
                {
                    sum += int.Parse(gameParts[0].Trim());
                }
            }

            return sum;
        }
    }
}
