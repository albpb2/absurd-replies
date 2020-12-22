using System;
using System.Threading.Tasks;
using AbsurdReplies.Dependencies;
using AbsurdReplies.Game.Round;
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
        
        private delegate Task QuestionCategoryReadyDelegate();
        private event QuestionCategoryReadyDelegate onQuestionCategoryReady;
        
        private delegate Task QuestionReadyDelegate();
        private event QuestionReadyDelegate onQuestionReady;

        [SerializeField] private TMP_Text _timerText;

        [SyncVar] private bool _started;
        [SyncVar] private bool _finished;
        [SyncVar(hook = nameof(UpdateTimerText))] private int _remainingSeconds;
        
        private QuestionCategorySelector _questionCategorySelector;
        private RoundReplies _roundReplies;
        private GameViewsManager _gameViewsManager;
        private GameQuestionsProvider _questionsProvider;
        private WaitingToStartRoundState _waitingToStartRoundState;
        private QuestionAndAnswerRoundState _questionAndAnswerRoundState;
        
        private DateTime? _startTime;
        private QuestionCategory _questionCategory;
        private Question _question;
        private IRoundState _state;

        public bool Started => _started;
        public bool Finished => _finished;
        public int RemainingSeconds => _remainingSeconds;
        public Question Question => _question;

        private int DurationSeconds => GameSettings.Instance.RoundTimeSeconds;
        
        [Inject]
        public void InitializeDependencies(
            QuestionCategorySelector questionCategorySelector,
            RoundReplies roundReplies,
            GameViewsManager gameViewsManager,
            GameQuestionsProvider questionsProvider,
            WaitingToStartRoundState waitingToStartRoundState,
            QuestionAndAnswerRoundState questionAndAnswerRoundState)
        {
            _questionCategorySelector = questionCategorySelector;
            _roundReplies = roundReplies;
            _gameViewsManager = gameViewsManager;
            _questionsProvider = questionsProvider;
            _waitingToStartRoundState = waitingToStartRoundState;
            _questionAndAnswerRoundState = questionAndAnswerRoundState;
            
            DependencyValidator.ValidateDependency(_questionCategorySelector, nameof(_questionCategorySelector), nameof(AbsurdRepliesRound));
            DependencyValidator.ValidateDependency(_roundReplies, nameof(_roundReplies), nameof(AbsurdRepliesRound));
            DependencyValidator.ValidateDependency(_gameViewsManager, nameof(_gameViewsManager), nameof(AbsurdRepliesRound));
            DependencyValidator.ValidateDependency(_questionsProvider, nameof(_questionsProvider), nameof(AbsurdRepliesRound));
            DependencyValidator.ValidateDependency(_waitingToStartRoundState, nameof(_waitingToStartRoundState), nameof(AbsurdRepliesRound));
            DependencyValidator.ValidateDependency(_questionAndAnswerRoundState, nameof(_questionAndAnswerRoundState), nameof(AbsurdRepliesRound));
        }

        private async void Awake()
        {
            DependencyValidator.ValidateDependency(_timerText, nameof(_timerText), nameof(AbsurdRepliesRound));
        }

        private void Start()
        {
            _state = _waitingToStartRoundState;
        }

        private async void Update()
        {
            if (!isServer)
            {
                return;
            }

            _state = await _state.Update(this);
        }

        private void OnEnable()
        {
            onQuestionCategoryReady += PickQuestion;
            onQuestionReady += StartRound;
        }

        public async Task PlayNewRound()
        {
            if (!isServer)
            {
                Destroy(gameObject);
            }

            Debug.Log("Initializing round");

            _started = false;
            _finished = false;
            _questionCategory = await _questionCategorySelector.SelectRandomQuestionCategory();
            Debug.Log($"Category picked: {_questionCategory}");
            if (_questionCategory == QuestionCategory.Unknown)
            {
                _gameViewsManager.DisplayCategorySelectionView();
            }
            else
            {
                await PickQuestion();
            }
        }

        [Command(ignoreAuthority = true)]
        public async void SetQuestionCategory(string questionCategory)
        {
            _questionCategory = (QuestionCategory)Enum.Parse(typeof(QuestionCategory), questionCategory);
            Debug.Log($"Category picked by round leader: {_questionCategory}");
            _gameViewsManager.HideCategorySelectionView();
            
            await PickQuestion();
        }

        public Task StartTimer()
        {
            _startTime = DateTime.UtcNow;
            _remainingSeconds = DurationSeconds;
            
            return Task.CompletedTask;
        }

        public Task UpdateRemainingTime()
        {
            var elapsedTime = DateTime.UtcNow - _startTime.Value;
            _remainingSeconds = DurationSeconds - (int) elapsedTime.TotalSeconds;
            if (_remainingSeconds < 0)
                _remainingSeconds = 0;

            return Task.CompletedTask;
        }

        private void UpdateTimerText(int oldValue, int newValue)
        {
            _timerText.text = $"{newValue}";
        }

        private async Task PickQuestion()
        {
            _question = await _questionsProvider.GetQuestion(_questionCategory);
            _roundReplies.SetTrueReply(_question.Answer);
            onQuestionReady?.Invoke();
        }

        public async Task StartRound()
        {
            _state = await _questionAndAnswerRoundState.EnterState(this);
            _started = true;
        }

        public Task FinishRound()
        {
            _gameViewsManager.HideAllViews();
            _started = false;
            onRoundFinished?.Invoke();

            return Task.CompletedTask;
        }
    }
}