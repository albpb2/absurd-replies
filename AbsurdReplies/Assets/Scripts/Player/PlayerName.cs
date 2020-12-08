using SPStudios.Tools;

namespace AbsurdReplies.Player
{
    public class PlayerName : MonoSingleton<PlayerName>
    {
        private string _playerName;

        public void Set(string playerName)
        {
            _playerName = playerName;
        }

        public string Get() => _playerName;
    }
}