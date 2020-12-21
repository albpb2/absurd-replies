﻿using System;
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

        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private GameViewsManager _gameViewsManager;
        [SerializeField] private GameQuestionsProvider _questionsProvider;
        
        [SyncVar] private bool _started;
        [SyncVar] private bool _finished;
        [SyncVar(hook = nameof(UpdateTimerText))] private int _remainingSeconds;
        
        private QuestionCategorySelector _questionCategorySelector;
        
        private DateTime? _startTime;
        private QuestionCategory _questionCategory;
        private Question _question;

        private int DurationSeconds => GameSettings.Instance.RoundTimeSeconds;
        
        [Inject]
        public void InitializeDependencies(QuestionCategorySelector questionCategorySelector)
        {
            _questionCategorySelector = questionCategorySelector;
        }

        private async void Awake()
        {
            DependencyValidator.ValidateDependency(_timerText, nameof(_timerText), nameof(AbsurdRepliesRound));
            DependencyValidator.ValidateDependency(_gameViewsManager, nameof(_gameViewsManager), nameof(AbsurdRepliesRound));
        }

        private async void Update()
        {
            if (!isServer)
            {
                return;
            }
            
            if (_started && !_finished)
            {
                UpdateRemainingTime();
                await FinishRoundIfTimeIsOver();
            }

            if (_started && _finished)
            {
                _started = false;
                await InitializeRound();
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

        [Command(ignoreAuthority = true)]
        public async void SetQuestionCategory(string questionCategory)
        {
            _questionCategory = (QuestionCategory)Enum.Parse(typeof(QuestionCategory), questionCategory);
            Debug.Log($"Category picked by round leader: {_questionCategory}");
            _gameViewsManager.HideCategorySelectionView();
            
            await PickQuestion();
            StartRound();
        }

        private void UpdateTimerText(int oldValue, int newValue)
        {
            _timerText.text = $"{newValue}";
        }

        private async Task InitializeRound()
        {
            Debug.Log("Initializing round");

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
                StartRound();
            }
        }

        private async Task PickQuestion()
        {
            _question = await _questionsProvider.GetQuestion(_questionCategory);
        }

        private void StartRound()
        {
            InitializeTimerText();
            _gameViewsManager.DisplayQuestionAndAnswerViews(_question);
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

        private Task FinishRoundIfTimeIsOver()
        {
            if (_remainingSeconds <= 0)
            {
                _remainingSeconds = 0;
                return FinishRound();
            }
            
            return Task.CompletedTask;
        }

        private Task FinishRound()
        {
            _finished = true;
            _gameViewsManager.HideAllViews();
            onRoundFinished?.Invoke();
            return Task.CompletedTask;
        }

        private void UpdateRemainingTime()
        {
            var elapsedTime = DateTime.UtcNow - _startTime.Value;
            _remainingSeconds = DurationSeconds - (int) elapsedTime.TotalSeconds;
        }

    }
}