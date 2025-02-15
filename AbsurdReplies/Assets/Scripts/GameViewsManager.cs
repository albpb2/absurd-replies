﻿using System.Threading.Tasks;
using AbsurdReplies.Dependencies;
using AbsurdReplies.Exceptions;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AbsurdReplies
{
    public class GameViewsManager : NetworkBehaviour
    {
        [SerializeField] private GameObject _categorySelectionView;
        [SerializeField] private GameObject _questionView;
        [SerializeField] private GameObject _answerView;
        [SerializeField] private GameObject _votingView;
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private TMP_Text _answerText;
        [SerializeField] private TMP_InputField _votingInputField;
        [SerializeField] private Button _votingButton;
        [SerializeField] private TMP_InputField _answerViewInputText;
        [SerializeField] private Button _answerViewButton;
        
        private AbsurdRepliesRound _round;
        private AbsurdRepliesGame _game;

        [Inject]
        public void InitializeDependencies(AbsurdRepliesRound round, AbsurdRepliesGame game)
        {
            _round = round ?? throw ExceptionBecause.MissingDependency(nameof(round));
            _game = game ?? throw ExceptionBecause.MissingDependency(nameof(game));
        }

        private void Awake()
        {
            DependencyValidator.ValidateDependency(_categorySelectionView, nameof(_categorySelectionView));
            DependencyValidator.ValidateDependency(_questionView, nameof(_questionView));
            DependencyValidator.ValidateDependency(_answerView, nameof(_answerView));
            DependencyValidator.ValidateDependency(_questionText, nameof(_questionText));
            DependencyValidator.ValidateDependency(_votingView, nameof(_votingView));
            DependencyValidator.ValidateDependency(_answerText, nameof(_answerText));
            DependencyValidator.ValidateDependency(_votingInputField, nameof(_votingInputField));
            DependencyValidator.ValidateDependency(_votingButton, nameof(_votingButton));
        }

        public void DisplayCategorySelectionView()
        {
            DisplayCategorySelectionView(_game.GetCurrentRoundLeaderConnectionToClient());
        }

        public void HideCategorySelectionView()
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

        public void DisplayQuestionAndAnswerViews(Question question)
        {
            DisplayQuestionView(question);
            DisplayAnswerView();
        }

        private void DisplayQuestionView(Question question)
        {
            DisplayQuestionView(_game.GetCurrentRoundLeaderConnectionToClient(), NetworkQuestion.From(question));
        }

        private void HideQuestionView()
        {
            HideQuestionView(_game.GetCurrentRoundLeaderConnectionToClient());
        }
        
        [TargetRpc]
        private void DisplayQuestionView(NetworkConnection networkConnection, NetworkQuestion question)
        {
            _questionView.SetActive(true);
            _questionText.text = question.Heading;
            _answerText.text = question.Answer;
        }
        
        [TargetRpc]
        private void HideQuestionView(NetworkConnection networkConnection)
        {
            _questionView.SetActive(false);
        }

        private void DisplayAnswerView()
        {
            foreach (var connectionToClient in _game.GetCurrentRoundParticipantsConnectionToClient())
            {
                DisplayAnswerView(connectionToClient);
            }
        }

        private void HideAnswerView()
        {
            foreach (var connectionToClient in _game.GetCurrentRoundParticipantsConnectionToClient())
            {
                HideAnswerView(connectionToClient);
            }
        }
        
        [TargetRpc]
        private void DisplayAnswerView(NetworkConnection networkConnection)
        {
            _answerView.SetActive(true);
            _answerViewInputText.interactable = true;
            _answerViewButton.interactable = true;
        }
        
        [TargetRpc]
        private void HideAnswerView(NetworkConnection networkConnection)
        {
            _answerView.SetActive(false);
        }

        [ClientRpc]
        public async void HideAllViews()
        {
            _votingInputField.text = string.Empty;
            _answerViewInputText.text = string.Empty;
            
            _answerView.SetActive(false);
            _questionView.SetActive(false);
            _categorySelectionView.SetActive(false);
            _votingView.SetActive(false);
        }

        public async Task DisplayVotingView()
        {
            DisplayVotingView(_game.GetCurrentRoundLeaderConnectionToClient(), false);
            foreach (var participantConnectionToClient in _game.GetCurrentRoundParticipantsConnectionToClient())
            {
                DisplayVotingView(participantConnectionToClient, transform);
            }
        }
        
        [TargetRpc]
        public async void DisplayVotingView(NetworkConnection networkConnection, bool canVote)
        {
            _votingView.SetActive(true);
            _votingInputField.interactable = canVote;
            _votingButton.interactable = canVote;
        }
    }
}