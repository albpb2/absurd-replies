using SPStudios.Tools;

namespace AbsurdReplies
{
    public class GameSettings : MonoSingleton<GameSettings>
    {
        private int DefaultMinPlayers = 2;
        private int DefaultTargetScore = 15;
        private int DefaultRoundTimeSeconds = 10;
        private int DefaultVotingTimeSeconds = 60;

        public int MinPlayers => DefaultMinPlayers;

        public int TargetScore => DefaultTargetScore;

        public int RoundTimeSeconds => DefaultRoundTimeSeconds;

        public int VotingTimeSeconds => 60;
    }
}