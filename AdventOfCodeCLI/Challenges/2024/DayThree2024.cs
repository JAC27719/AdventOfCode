using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCodeCLI.Challenges.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdventOfCodeCLI.Challenges._2024
{
    internal class DayThree2024 : IChallengeSolver
    {

        private readonly ILogger _logger;

        public DayThree2024(ILogger logger) 
        {
            _logger = logger;
        }

        public async Task Solve(HttpClient httpClient)
        {
            _logger.LogInformation("Solving problem for day three of 2024");
            var res = await httpClient.GetAsync(httpClient.BaseAddress);
            var content = await res.Content.ReadAsStringAsync();

            StringBuilder sb = new StringBuilder();
            using (StringReader reader = new StringReader(content))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.Append(line.Replace(Environment.NewLine, ""));
                }
            }

            string pattern = @"(mul\(\d{1,3},\d{1,3}\))";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(sb.ToString());

            #region Part 1:
            long sumOfMuls = 0;
            foreach (Match match in matches)
            {
                //_logger.LogInformation($"Found match: {match.Value}");
                var mulParams = GetMulParameters(match.Value);

                sumOfMuls += (mulParams[0] * mulParams[1]);
            }

            _logger.LogInformation($"Sum of multiplications: {sumOfMuls}");
            #endregion

            #region Part 2:
            string patternWithConditions = @"(mul\(\d{1,3},\d{1,3}\))|(don't\(\))|(do\(\))";
            Regex regexWithConditions = new Regex(patternWithConditions);
            MatchCollection matchesWithConditions = regexWithConditions.Matches(sb.ToString());

            long sumOfMulsEnabled = 0;
            bool enabled = true;
            foreach (Match match in matchesWithConditions)
            {
                //_logger.LogInformation($"Found match: {match.Value}");

                bool isMulOp = true;
                if (match.Value.Equals("don't()"))
                {
                    enabled = false;
                    isMulOp = false;
                }


                if (match.Value.Equals("do()"))
                {
                    enabled = true;
                    isMulOp = false;
                }

                if (enabled && isMulOp)
                {
                    var mulParams = GetMulParameters(match.Value);

                    sumOfMulsEnabled += (mulParams[0] * mulParams[1]);

                }
            }

            _logger.LogInformation($"Sum of enabled multiplications: {sumOfMulsEnabled}");
            #endregion

        }

        private int[]  GetMulParameters(string mul) 
        {
            var strippedInstructions = mul.Remove(0, 4);
            strippedInstructions = strippedInstructions.Remove(strippedInstructions.Length - 1);
            int[] mulParams = new int[2];
            mulParams = strippedInstructions.Split(",").Select(i => int.Parse(i)).ToArray();

            //_logger.LogInformation($"Stripped instructions: {strippedInstructions}");

            return mulParams;
        }
    }
}
