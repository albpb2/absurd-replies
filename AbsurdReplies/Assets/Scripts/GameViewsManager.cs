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
            _round.onKnownCategoryPicked += HideCategorySelectionView;
        }
        async void OnDisable()
        {
            _round.onUnknownCategoryPicked -= DisplayCategorySelectionView;
            _round.onKnownCategoryPicked -= HideCategorySelectionView;
        }

        private void DisplayCategorySelectionView()
        {
            DisplayCategorySelectionView(_game.GetCurrentRoundLeaderConnectionToClient());
        }

        private void HideCategorySelectionView()
        {
            HideCategorySelectionView(_game.GetCurrentRoundLeaderConnectionToClient());
        }
        
        [TargetRpc]
        private void DisplayCategorySelectionView(NetworkConnection networkConnection)
        {
            _categorySelectionView.SetActive(true);
        }
        
        [TargetRpc]
        private void HideCategorySelectionView(NetworkConnection networkConnection)
        {
            _categorySelectionView.SetActive(false);
        }
    }
}