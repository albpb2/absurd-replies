using AbsurdReplies.Exceptions;
using UnityEngine;
using UnityEngine.UI;

namespace AbsurdReplies
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_InputField _gameIdSource;

        public string GameId => _gameIdSource.text;
        
        private void Awake()
        {
            DependencyValidator.ValidateDependency(_gameIdSource, nameof(_gameIdSource), nameof(GameStarter));
        }

        public async void StartHost()
        {
            if (string.IsNullOrWhiteSpace(GameId))
            {
                throw ExceptionBecause.GameIdMissing();
            }
        }

        public async void JoinLocal()
        {
            if (string.IsNullOrWhiteSpace(GameId))
            {
                throw ExceptionBecause.GameIdMissing();
            }
        }
    }
}