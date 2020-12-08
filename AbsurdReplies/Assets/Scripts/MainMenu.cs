using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AbsurdReplies
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] 
        private TMP_InputField _playerNameInput;
        [SerializeField] 
        private TMP_InputField _portInput;
        [SerializeField] 
        private Button[] _localGameButtons;

        private void Awake()
        {
            DependencyValidator.ValidateDependency(_playerNameInput, nameof(_playerNameInput), nameof(MainMenu));
            DependencyValidator.ValidateDependency(_portInput, nameof(_portInput), nameof(MainMenu));
            DependencyValidator.ValidateDependency(_localGameButtons, nameof(_localGameButtons), nameof(MainMenu));
        }

        private async void Update()
        {
            await UpdateLocalGameButtons();
        }

        private Task UpdateLocalGameButtons()
        {
            return EnableOrDisableButtons(ShouldLocalGameButtonsBeEnabled(), _localGameButtons);
        }

        private static Task EnableOrDisableButtons(bool enable, Button[] buttons) => 
            ChangeButtonsInteractability(enable, buttons);

        private static Task ChangeButtonsInteractability(bool interactable, Button[] buttons)
        {
            if (buttons == null)
            {

                return Task.CompletedTask;
            }

            foreach (var gameIdDependentButton in buttons)
            {
                gameIdDependentButton.interactable = interactable;
            }

            return Task.CompletedTask;
        }

        private bool ShouldLocalGameButtonsBeEnabled() =>
            _playerNameInput.text.HasValue() && _portInput.text.HasValue();
    }
}