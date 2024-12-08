using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeCLI.Challenges.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdventOfCodeCLI.Challenges._2024
{
    internal class DayTwo2024 : IChallengeSolver
    {

        private readonly ILogger _logger;
        public DayTwo2024(ILogger logger) 
        {
            _logger = logger;
        }

        public Task Solve(HttpClient httpClient)
        {
            throw new NotImplementedException();
        }
    }
}
