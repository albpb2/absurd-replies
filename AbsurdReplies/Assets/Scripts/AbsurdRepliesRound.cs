using System;
using System.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;
using Zenject;

namespace AbsurdReplies
{
    public class AbsurdRepliesRound : NetworkBehaviour
    {
        public delegate void RoundFinishedDelegate();
        public event RoundFinishedDelegate onRoundFinished;

        public delegate void UnknownCategoryPickedDelegate();
        public event UnknownCategoryPickedDelegate onUnknownCategoryPicked;

        public delegate void KnownCategoryPickedDelegate();
        public event KnownCategoryPickedDelegate onKnownCategoryPicked;

        [SerializeField] private TMP_Text _timerText;
        
        [SyncVar] private bool _started;
        [SyncVar] private bool _finished;
        [SyncVar] private int _remainingSeconds;
        
        private QuestionCategorySelector _questionCategorySelector;
        
        private DateTime? _startTime;
        private QuestionCategory _questionCategory;

        private int DurationSeconds => GameSettings.Instance.RoundTimeSeconds;
        
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
            if (_started)
            {
                _timerText.text = $"{_remainingSeconds}";
            }
            
            if (!isServer)
            {
                return;
            }
            
            if (_started && !_finished)
            {
                UpdateRemainingTime();
                FinishRoundIfTimeIsOver();
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

        public void SetQuestionCategory(string questionCategory)
        {
            _questionCategory = (QuestionCategory)Enum.Parse(typeof(QuestionCategory), questionCategory);
            Debug.Log($"Category picked by round leader: {_questionCategory}");
            onKnownCategoryPicked?.Invoke();
            StartRound();
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
            if (_questionCategory == QuestionCategory.Unknown)
            {
                onUnknownCategoryPicked?.Invoke();
            }
            else
            {
                StartRound();
            }
        }

        private void StartRound()
        {
            InitializeTimerText();
            StartTimer();
            _started = true;
        }

        private void InitializeTimerText()
        {
            _remainingSeconds = DurationSeconds;
        }

        private Task StartTimer()
        {
            _startTime = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        private void FinishRoundIfTimeIsOver()
        {
            if (_remainingSeconds <= 0)
            {
                _remainingSeconds = 0;
                _finished = true;
                onRoundFinished?.Invoke();
            }
        }

        private void UpdateRemainingTime()
        {
            var elapsedTime = DateTime.UtcNow - _startTime.Value;
            _remainingSeconds = DurationSeconds - (int) elapsedTime.TotalSeconds;
        }

    }
}