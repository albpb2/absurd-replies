using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbsurdReplies.Dependencies;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AbsurdReplies
{
    public class VotingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _optionsText;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _button;

        private VotingProcess _votingProcess;

        private HashSet<string> _validStrings;

        [Inject]
        public void InitializeDependencies(VotingProcess votingProcess)
        {
            _votingProcess = votingProcess;
        }

        private async void Awake()
        {
            _validStrings = new HashSet<string>();
            
            DependencyValidator.ValidateDependency(_optionsText, nameof(_optionsText), nameof(VotingView));
            DependencyValidator.ValidateDependency(_inputField, nameof(_inputField), nameof(VotingView));
            DependencyValidator.ValidateDependency(_button, nameof(_button), nameof(VotingView));
        }

        public async void Vote()
        {
            if (string.IsNullOrWhiteSpace(_inputField.text))
                return;

            var option = _inputField.text.ToUpper();
            if (!_validStrings.Contains(option))
                return;

            DisableControls();
            
            _votingProcess.Vote(option);
        }

        public async void DisplayOptionsAndReplies(string formattedOptionsAndRepliesText)
        {
            _optionsText.text = formattedOptionsAndRepliesText;
        }

        public void SetValidOptions(IEnumerable<string> options)
        {
            _validStrings = new HashSet<string>(options);
        }

        private void DisableControls()
        {
            _inputField.interactable = false;
            _button.interactable = false;
        }
    }
}