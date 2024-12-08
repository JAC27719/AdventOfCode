using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCodeCLI.Challenges.Enums.ChallengeEnums;

namespace AdventOfCodeCLI.Challenges.Interfaces
{
    internal interface IChallengeSolver
    {
        Task Solve(HttpClient httpClient);
    }
}
