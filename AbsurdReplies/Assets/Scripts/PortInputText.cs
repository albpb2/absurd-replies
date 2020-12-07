using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AbsurdReplies
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PortInputText : MonoBehaviour
    {
        private const ushort DefaultPort = 7777;
        
        private TMP_InputField _inputField;
        
        [SerializeField]
        private Button[] _gameIdDependentButtons;

        public ushort Port => string.IsNullOrEmpty(_inputField.text) ? DefaultPort : ushort.Parse(_inputField.text);

        private async void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        public async void UpdateGameIdText()
        {
            if (string.IsNullOrWhiteSpace(_inputField.text))
            {
                await DisableButtons();
            }
            else
            {
                await EnableButtons();
            }
        }

        private Task DisableButtons() => ChangeButtonsInteractability(false);

        private Task EnableButtons() => ChangeButtonsInteractability(true);

        private Task ChangeButtonsInteractability(bool interactable)
        {
            if (_gameIdDependentButtons == null)
            {

                return Task.CompletedTask;
            }

            foreach (var gameIdDependentButton in _gameIdDependentButtons)
            {
                gameIdDependentButton.interactable = interactable;
            }

            return Task.CompletedTask;
        }
    }
}