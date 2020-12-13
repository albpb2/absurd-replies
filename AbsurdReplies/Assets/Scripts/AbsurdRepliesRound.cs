using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

namespace AbsurdReplies
{
    public class AbsurdRepliesRound : NetworkBehaviour
    {
        public delegate void RoundFinishedDelegate();
        public event RoundFinishedDelegate onRoundFinished;

        private QuestionCategorySelector _questionCategorySelector;
        
        private DateTime? _startTime;
        private bool _finished;
        private QuestionCategory _questionCategory;

        private int DurationSeconds => GameSettings.Instance.RoundTimeSeconds;
        private bool Started => _startTime.HasValue;
        private bool Finished => _finished;
        
        [Inject]
        public void InitializeDependencies(QuestionCategorySelector questionCategorySelector)
        {
            _questionCategorySelector = questionCategorySelector;
        }

        private async void Awake()
        {
            DependencyValidator.ValidateDependency(_questionCategorySelector, nameof(_questionCategorySelector), nameof(AbsurdRepliesRound));
        }

        private async void Update()
        {
            if (Started && !Finished)
            {
                if (_startTime.Value.AddSeconds(DurationSeconds) < DateTime.UtcNow)
                {
                    _finished = true;
                    onRoundFinished?.Invoke();
                }
            }
        }

        public async Task PlayNewRound()
        {
            if (!isServer)
            {
                Destroy(gameObject);
            }

            await InitializeRound();
        }

        private async Task InitializeRound()
        {
            Debug.Log("Initializing round");

            await PickCategory();
        }

        private async Task PickCategory()
        {
            _questionCategory = await _questionCategorySelector.SelectRandomQuestionCategory();
            Debug.Log($"Category picked: {_questionCategory}");
        }

        private Task StartTimer()
        {
            _startTime = DateTime.UtcNow;
            return Task.CompletedTask;
        }
    }
}