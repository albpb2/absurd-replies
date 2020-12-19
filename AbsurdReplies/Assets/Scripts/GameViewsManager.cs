using Mirror;
using UnityEngine;

namespace AbsurdReplies
{
    public class GameViewsManager : NetworkBehaviour
    {
        [SerializeField] private GameObject _categorySelectionView;
        [SerializeField] private AbsurdRepliesRound _round;
        [SerializeField] private AbsurdRepliesGame _game;
        
        async void OnEnable()
        {
            _round.onUnknownCategoryPicked += DisplayCategorySelectionView;
        }

        async void OnDisable()
        {
            _round.onUnknownCategoryPicked -= DisplayCategorySelectionView;
        }

        private void DisplayCategorySelectionView()
        {
            DisplayCategorySelectionView(_game.GetCurrentRoundLeaderConnectionToClient());
        }

        [TargetRpc]
        private void DisplayCategorySelectionView(NetworkConnection networkConnection)
        {
            _categorySelectionView.SetActive(true);
        }
    }
}