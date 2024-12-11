using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeCLI.Challenges;
using AdventOfCodeCLI.Challenges.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AdventOfCodeCLI.Challenges.Enums.ChallengeEnums;

namespace AdventOfCodeCLI
{

    internal class Worker
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;
        private HttpClient _httpClient;
        
        public Worker(IConfiguration configuration, ILogger<Worker> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Hello, Advent of Code!");

            ChallengeDay challengeDay = _configuration.GetValue<ChallengeDay>("challenge");

            //Defaulting challenge year to 2024 for now...
            ChallengeYear challengeYear = ChallengeYear.TwentyTwentyFour;

            InputMethod inputMethod = _configuration.GetValue<InputMethod>("inputMethod");
            string basePath = _configuration.GetValue<string>("baseInputUrl");
            
            IChallengeSolver challengeSolver = new ChallengeSolver(challengeDay, challengeYear, _logger);

            string inputPath = $"{basePath}/{(short)challengeYear}/day/{(byte)challengeDay}/input";

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(inputPath),
            };

            string session = Environment.GetEnvironmentVariable("session");
            _httpClient.DefaultRequestHeaders.Add("cookie", $"session={session}");

            challengeSolver.Solve(_httpClient).Wait();
        }
    }
 }
