using SPStudios.Tools;

namespace AbsurdReplies
{
    public class GameSettings : MonoSingleton<GameSettings>
    {
        private int DefaultMinPlayers = 2;

        public int MinPlayers => DefaultMinPlayers;
    }
}