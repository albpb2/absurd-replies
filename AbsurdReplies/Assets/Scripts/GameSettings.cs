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

        [SerializeField] private int _minPlayers = DefaultMinPlayers;
        [SerializeField] private int _targetScore = DefaultTargetScore;
        [SerializeField] private int _roundTimeSeconds = DefaultRoundTimeSeconds;
        [SerializeField] private int _votingTimeSeconds = DefaultVotingTimeSeconds;

        public int MinPlayers => _minPlayers;

        public int TargetScore => _targetScore;

        public int RoundTimeSeconds => _roundTimeSeconds;

        public int VotingTimeSeconds => _votingTimeSeconds;
    }
}