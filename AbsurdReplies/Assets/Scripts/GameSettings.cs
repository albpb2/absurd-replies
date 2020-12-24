using SPStudios.Tools;
using UnityEngine;

namespace AbsurdReplies
{
    public class GameSettings : MonoSingleton<GameSettings>
    {
        private const int DefaultMinPlayers = 1;
        private const int DefaultTargetScore = 15;
        private const int DefaultRoundTimeSeconds = 10;
        private const int DefaultVotingTimeSeconds = 60;
        private const int DefaultPointsPerCorrectGuess = 2;
        private const int DefaultPointsPerVoteReceived = 1;
        private const int DefaultPointForNotGuessedAnswer = 3;

        [SerializeField] private int _minPlayers = DefaultMinPlayers;
        [SerializeField] private int _targetScore = DefaultTargetScore;
        [SerializeField] private int _roundTimeSeconds = DefaultRoundTimeSeconds;
        [SerializeField] private int _votingTimeSeconds = DefaultVotingTimeSeconds;
        [SerializeField] private int _pointsPerCorrectGuess = DefaultPointsPerCorrectGuess;
        [SerializeField] private int _pointsPerVoteReceived = DefaultPointsPerVoteReceived;
        [SerializeField] private int _pointsForNotGuessedAnswer = DefaultPointForNotGuessedAnswer;

        public int MinPlayers => _minPlayers;

        public int TargetScore => _targetScore;

        public int RoundTimeSeconds => _roundTimeSeconds;

        public int VotingTimeSeconds => _votingTimeSeconds;

        public int PointsPerCorrectGuess => _pointsPerCorrectGuess;

        public int PointsPerVoteReceived => _pointsPerVoteReceived;

        public int PointsForNotGuessedAnswer => _pointsForNotGuessedAnswer;
    }
}