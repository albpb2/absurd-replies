using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AbsurdReplies
{
    public class GetCodeButton : MonoBehaviour
    {
        [SerializeField] private Text _gameCodeText;
        
        private GameCodeRetriever _gameCodeRetriever;
        
        [Inject]
        public void InitializeDependencies(GameCodeRetriever gameCodeRetriever)
        {
            _gameCodeRetriever = gameCodeRetriever;
        }

        public async void GetGameCode()
        {
            var gameCode = await _gameCodeRetriever.RetrieveGameCode();
            _gameCodeText.text = gameCode;
        }
    }
}

