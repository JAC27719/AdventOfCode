using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCLI.Challenges.Enums
{
    internal class ChallengeEnums
    {
        internal enum ChallengeDay : byte
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
        }

        internal enum ChallengeYear: short
        {
            TwentyTwentyFour = 2024
        }

        internal enum InputMethod: byte
        {
            Unknown = 0,
            API = 1,
            File
        }
    }
}
