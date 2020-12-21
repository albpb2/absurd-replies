using System;
using TMPro;
using UnityEngine;

namespace AbsurdReplies
{
    public class ReplyView : MonoBehaviour
    {
        [SerializeField] private RoundReplies _roundReplies;
        [SerializeField] private TMP_InputField _replyInputField;

        private void Awake()
        {
            DependencyValidator.ValidateDependency(_roundReplies, nameof(_roundReplies), nameof(ReplyView));
            DependencyValidator.ValidateDependency(_replyInputField, nameof(_replyInputField), nameof(ReplyView));
        }

        public async void SendReply()
        {
            if (string.IsNullOrEmpty(_replyInputField.text))
            {
                return;
            }
            
            _roundReplies.SendReply(_replyInputField.text);
        }
    }
}