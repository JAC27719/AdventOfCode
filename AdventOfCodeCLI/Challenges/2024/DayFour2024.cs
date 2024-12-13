using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCodeCLI.Challenges.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdventOfCodeCLI.Challenges._2024
{
    internal class DayFour2024 : IChallengeSolver
    {

        private readonly ILogger _logger;

        public DayFour2024(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Solve(HttpClient httpClient)
        {
            _logger.LogInformation("Solving problem for day four of 2024");
            var res = await httpClient.GetAsync(httpClient.BaseAddress);
            var content = await res.Content.ReadAsStringAsync();

            //content = "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX";

            List<string> horizontalLines = new List<string>();
            using (StringReader reader = new StringReader(content))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    horizontalLines.Add(line);
                }
            }

            List<string> verticalLines = new List<string>();
            List<string> diagonalLinesRight = new List<string>();
            List<string> diagonalLinesLeft = new List<string>();

            //Create list of vertical lines
            for (int l = 0; l < horizontalLines.Count; l++)
            {
                int lineLength = horizontalLines[l].Length;

                for (int c = 0; c < lineLength; c++)
                {
                    if (l == 0)
                    {
                        verticalLines.Add("");
                        verticalLines[c] += horizontalLines[l][c];
                    }
                    else
                    {
                        verticalLines[c] += horizontalLines[l][c];
                    }
                }
            }

            //Create list of diagonal lines going right
            diagonalLinesRight = GetDiagonals(horizontalLines, verticalLines);
            

            
            //Create list of diagonal lines going left
            List<string> horizontalLinesReversed = new List<string>();
            for (int i = 0; i < horizontalLines.Count; i++)
            {
                var reversedLine = horizontalLines[i].Reverse();
                var reversedString = "";
                for (int j = 0; j < reversedLine.Count(); j++) 
                {
                    reversedString += reversedLine.ElementAt(j);
                }

                horizontalLinesReversed.Add(reversedString);
            }

            List<string> verticalLinesReversed = new List<string>();
            verticalLinesReversed = verticalLines.Reverse<string>().ToList();
            diagonalLinesLeft = GetDiagonals(horizontalLinesReversed,verticalLinesReversed);



            #region Part 1:
            string patternSMAX = @"(SAMX)";
            string patternXMAS = @"(XMAS)";

            Regex regexSMAX = new Regex(patternSMAX);
            Regex regexXMAS = new Regex(patternXMAS);
            int numMatches = 0;

            foreach (var line in horizontalLines)
            {
                MatchCollection matchesSMAX = regexSMAX.Matches(line);
                MatchCollection matchesXMAS = regexXMAS.Matches(line);

                numMatches += matchesSMAX.Count + matchesXMAS.Count;
            }
            foreach (var line in verticalLines)
            {
                MatchCollection matchesSMAX = regexSMAX.Matches(line);
                MatchCollection matchesXMAS = regexXMAS.Matches(line);

                numMatches += matchesSMAX.Count + matchesXMAS.Count;
            }
            foreach (var line in diagonalLinesRight)
            {
                MatchCollection matchesSMAX = regexSMAX.Matches(line);
                MatchCollection matchesXMAS = regexXMAS.Matches(line);

                numMatches += matchesSMAX.Count + matchesXMAS.Count;
            }
            foreach (var line in diagonalLinesLeft)
            {
                MatchCollection matchesSMAX = regexSMAX.Matches(line);
                MatchCollection matchesXMAS = regexXMAS.Matches(line);

                numMatches += matchesSMAX.Count + matchesXMAS.Count;
            }

            _logger.LogInformation($"# of XMAS occurences: {numMatches}");
            #endregion

            #region Part 2:
            string patternMAS = @"(MAS)";
            string patternSAM = @"(SAM)";

            Regex regexMAS = new Regex(patternMAS);
            Regex regexSAM = new Regex(patternSAM);
            numMatches = 0;
            int gridWidth = horizontalLines[0].Length;

            for (int i = 0; i < diagonalLinesRight.Count; i++)
            {
                MatchCollection matchesMAS = regexMAS.Matches(diagonalLinesRight[i]);
                MatchCollection matchesSAM = regexSAM.Matches(diagonalLinesRight[i]);

                numMatches += GetX_MasMatches(matchesMAS, gridWidth, diagonalLinesRight, i);
                numMatches += GetX_MasMatches(matchesSAM, gridWidth, diagonalLinesRight, i);
            }
            
            _logger.LogInformation($"# of X-MAS occurences: {numMatches}");
            #endregion
        }

        private List<string> GetDiagonals(List<string> horizontalLines, List<string> verticalLines)
        {
            List<string> diagonalLines = new List<string>();

            for (int h = 0; h < horizontalLines.Count; h++)
            {
                for (int v = 0; v < verticalLines.Count; v++)
                {
                    if (h == 0)
                    {
                        diagonalLines.Add("");
                        diagonalLines[v + h] += horizontalLines[h][v];
                    }
                    else if (v == 0)
                    {
                        diagonalLines.Add("");
                        diagonalLines[(horizontalLines.Count - 1) + h] += verticalLines[v][h];
                    }
                    else
                    {
                        if ((v - h) > 0)
                        {
                            diagonalLines[v - h] += verticalLines[v][h];
                        }
                        else if ((v - h) < 0)
                        {
                            diagonalLines[(horizontalLines.Count - 1) + (h - v)] += verticalLines[v][h];
                        }
                        else
                        {
                            diagonalLines[0] += verticalLines[v][h];
                        }
                    }
                }
            }

            return diagonalLines;
        }

        private int GetX_MasMatches(MatchCollection matchCollection, int gridWidth, List<string> diagonalLinesRight, int i)
        {
            if (i == 15)
            {
                _logger.LogInformation("i is 15!");
            }
            int numMatches = 0;
            foreach (Match match in matchCollection)
            {
                int matchIdxOfA = match.Index + 1; //character position in string at horizontalLines[i]
                
                string rightDiagonal = "";
                string leftDiagonal = "";

                int rightDiagonalCharIndex = 0;
                int leftDiagonalCharIndex = 0;

                if (i <= 1)
                {
                    rightDiagonal = diagonalLinesRight[i + 2];
                    leftDiagonal = diagonalLinesRight[(gridWidth + 1) - i];
                    if (i == 0)
                    {
                        leftDiagonalCharIndex = matchIdxOfA - 1;
                    }
                    else
                    {
                        leftDiagonalCharIndex = matchIdxOfA;
                    }

                    rightDiagonalCharIndex = matchIdxOfA - 1;
                }
                else if (i > 1 && i < gridWidth)
                {
                    rightDiagonal = diagonalLinesRight[i + 2];
                    leftDiagonal = diagonalLinesRight[i - 2];

                    rightDiagonalCharIndex = matchIdxOfA - 1;
                    leftDiagonalCharIndex = matchIdxOfA + 1;
                }
                else if (i > (gridWidth - 1))
                {
                    if (i <= (gridWidth) + 1)
                    {
                        rightDiagonal = diagonalLinesRight[(gridWidth + 1) - i];

                        if (i == gridWidth) 
                        {
                            rightDiagonalCharIndex = matchIdxOfA;
                        }
                        else
                        {
                            rightDiagonalCharIndex = matchIdxOfA + 1;
                        }
                    }
                    else
                    {
                        rightDiagonal = diagonalLinesRight[i - 2];
                        rightDiagonalCharIndex = matchIdxOfA + 1;
                    }

                    leftDiagonal = diagonalLinesRight[i + 2];
                    leftDiagonalCharIndex = matchIdxOfA - 1;
                }

                
                if (!(rightDiagonal[rightDiagonalCharIndex].Equals('X'))
                    && !(rightDiagonal[rightDiagonalCharIndex].Equals('A'))
                    && !(leftDiagonal[leftDiagonalCharIndex].Equals('X'))
                    && !(leftDiagonal[leftDiagonalCharIndex].Equals('A'))
                    && rightDiagonal[rightDiagonalCharIndex] != leftDiagonal[leftDiagonalCharIndex])
                {
                    //_logger.LogInformation($"Right: {rightDiagonal[rightDiagonalCharIndex]} ; Left: {leftDiagonal[leftDiagonalCharIndex]}");
                    _logger.LogInformation(
                        $"{match.Value[0]} . {rightDiagonal[rightDiagonalCharIndex]}\r\n" +
                        $". {match.Value[1]} .\r\n" +
                        $"{leftDiagonal[leftDiagonalCharIndex]} . {match.Value[2]}");
                    _logger.LogInformation($"Diagonal index: {i} ; A index: {matchIdxOfA}");
                    numMatches++;
                }
            }

            return numMatches;
        }
    }
}

