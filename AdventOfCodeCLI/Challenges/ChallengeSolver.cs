using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeCLI.Challenges._2024;
using AdventOfCodeCLI.Challenges.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AdventOfCodeCLI.Challenges.Enums.ChallengeEnums;

namespace AdventOfCodeCLI.Challenges
{
    internal class ChallengeSolver : IChallengeSolver
    {
        private readonly ILogger _logger;

        private ChallengeDay _challengeDay;
        public ChallengeDay ChallengeDay {
            get => _challengeDay;
            private set 
            {
                _challengeDay = value;
            } 
        }

        private ChallengeYear _challengeYear;
        public ChallengeYear ChallengeYear
        {
            get => _challengeYear;
            private set
            {
                _challengeYear = value;
            }
        }

        public ChallengeSolver(ChallengeDay challengeDay, ChallengeYear challengeYear, ILogger logger)
        {
            _challengeDay = challengeDay; 
            _challengeYear = challengeYear;
            _logger = logger;
        }


        public async Task Solve(HttpClient httpClient)
        {
            IChallengeSolver solver;
            StringBuilder sb = new StringBuilder();

            switch(ChallengeYear)
            {
                case ChallengeYear.TwentyTwentyFour:
                    switch(ChallengeDay)
                    {
                        case ChallengeDay.One:
                            solver = new DayOne2024(_logger);
                            break;
                        default:
                            throw new NotImplementedException("Challenge has not yet been implemented for the given day in 2024!");
                    }
                    break;
                default:
                    throw new NotImplementedException("No challenges have been implemented for the given year!");
            }

            //Solve the problem
            await solver.Solve(httpClient);
        }
    }
}
