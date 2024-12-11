using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeCLI.Challenges.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdventOfCodeCLI.Challenges._2024
{
    internal class DayTwo2024 : IChallengeSolver
    {

        private class Report
        {
            private List<int> _levels;
            public Report(List<int> levels)
            {
                _levels = levels;
            }
            private bool IsLevelSafe(List<int> levels)
            {
                var distancesBetweenEach = new List<int>();

                for (int i = 0; i < levels.Count - 1; i++)
                {
                    int distanceBetween = levels[i + 1] - levels[i];
                    distancesBetweenEach.Add(distanceBetween);
                }

                int numIncreasing = distancesBetweenEach.Where(d => d > 0).Count();
                int numDecreasing = distancesBetweenEach.Where(d => d < 0).Count();

                bool areAllDistancesSafe = distancesBetweenEach.All(d => (Math.Abs(d) >= 1 && Math.Abs(d) <= 3));

                if (numIncreasing == distancesBetweenEach.Count && numDecreasing == distancesBetweenEach.Count)
                {
                    Debug.Fail("Both cannot be true at the same time!");
                }

                if (areAllDistancesSafe
                    && (numIncreasing == distancesBetweenEach.Count || numDecreasing == distancesBetweenEach.Count))
                {
                    return true;
                }

                return false;

            }
            public bool IsSafeWithoutProblemDampener()
            {
                return IsLevelSafe(_levels);
            }
            public bool IsSafeWithProblemDampener()
            {
                if (IsLevelSafe(_levels))
                    return true;

                for (int i = 0; i < _levels.Count; i++)
                {
                    var reducedLevels = _levels.Where((l,index) => index != i).ToList();

                    bool isSafe = IsLevelSafe(reducedLevels);
                    if (isSafe)
                        return true;
                }

                return false;
            }
        }

        private readonly ILogger _logger;

        public DayTwo2024(ILogger logger) 
        {
            _logger = logger;
        }

        public async Task Solve(HttpClient httpClient)
        {
            _logger.LogInformation("Solving problem for day two of 2024");
            var res = await httpClient.GetAsync(httpClient.BaseAddress);
            var content = await res.Content.ReadAsStringAsync();

            List<Report> reports = new List<Report>();
            using (StringReader reader = new StringReader(content))
            {
                string line;
                string delimeter = " ";
                while ((line = reader.ReadLine()) != null)
                {
                    var splitLines = line.Split(delimeter);

                    reports.Add(new Report(splitLines.Select(int.Parse).ToList()));
                }
            }

            #region Part 1: Calculate the number of safe levels in the reports
            int numSafeLevels = 0;
            foreach (var report in reports)
            {
                if (report.IsSafeWithoutProblemDampener())
                {
                    numSafeLevels++;
                }
            }

            _logger.LogInformation($"Number of safe levels: {numSafeLevels}");
            #endregion

            #region Part 2: Calculate the number of safe levels in the reports accounting for the Problem Dampener
            int numSafeLevelsWithDampener = 0;
            foreach (var report in reports)
            {
                if (report.IsSafeWithProblemDampener())
                {
                    numSafeLevelsWithDampener++;
                }
            }

            _logger.LogInformation($"Number of safe levels with problem dampener: {numSafeLevelsWithDampener}");
            #endregion
        }
    }
}
