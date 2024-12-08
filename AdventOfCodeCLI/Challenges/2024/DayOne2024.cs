using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeCLI.Challenges.Enums;
using AdventOfCodeCLI.Challenges.Interfaces;
using Microsoft.Extensions.Logging;
using static AdventOfCodeCLI.Challenges.Enums.ChallengeEnums;

namespace AdventOfCodeCLI.Challenges._2024
{
    internal class DayOne2024 : IChallengeSolver
    {
        private readonly ILogger _logger;
        public DayOne2024(ILogger logger) 
        {
            _logger = logger;
        }

        public async Task Solve(HttpClient httpClient)
        {
            _logger.LogInformation("Solving problem for day one of 2024");
            var res = await httpClient.GetAsync(httpClient.BaseAddress);
            var content = await res.Content.ReadAsStringAsync();


            List<int> locationIdsSetOne = new List<int>();
            List<int> locationIdsSetTwo = new List<int>();
            Dictionary<int, int> locationIdOccurencesSetTwo = new Dictionary<int, int>();
            using (StringReader reader = new StringReader(content))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var splitLines = line.Split("   ");

                    Debug.Assert(splitLines.Length == 2);

                    //Store dataset into two lists
                    locationIdsSetOne.Add(Int32.Parse(splitLines[0]));
                    locationIdsSetTwo.Add(Int32.Parse(splitLines[1]));
                }
            }


            Debug.Assert(locationIdsSetOne.Count == locationIdsSetTwo.Count);

            //Sort each list
            locationIdsSetOne.Sort();
            locationIdsSetTwo.Sort();

            #region Part 1: Calculate Distance Between Points
            int totalDistance = 0;
            for (int i = 0; i < locationIdsSetOne.Count; i++)
            {
                totalDistance += Math.Abs(locationIdsSetOne[i] - locationIdsSetTwo[i]); 
            }

            _logger.LogInformation($"Total distance: {totalDistance}");
            #endregion


            #region Part 2: Calculate Similarity Score
            for (int i = 0; i < locationIdsSetTwo.Count; i++)
            {
                int id = locationIdsSetTwo[i];
                if (locationIdOccurencesSetTwo.ContainsKey(id))
                {
                    locationIdOccurencesSetTwo[id]++;
                }
                else
                {
                    locationIdOccurencesSetTwo.Add(id, 1);
                }
            }

            int totalSimilarity = 0;
            foreach (int id in locationIdsSetOne)
            {
                var multiplier = 0;
                if (locationIdOccurencesSetTwo.ContainsKey(id))
                {
                    multiplier = locationIdOccurencesSetTwo[id];
                } 

                totalSimilarity += (id * multiplier);
            }

            _logger.LogInformation($"Total similarity: {totalSimilarity}");
            #endregion

        }
    }
}
