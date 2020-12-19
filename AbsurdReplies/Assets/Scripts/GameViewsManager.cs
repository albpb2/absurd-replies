using UnityEngine;

namespace AbsurdReplies
{
    public class GameViewsManager : MonoBehaviour
    {
        [SerializeField] private GameObject _categorySelectionView;
        [SerializeField] private AbsurdRepliesRound _round;
        
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
            _categorySelectionView.SetActive(true);
        }
    }
}