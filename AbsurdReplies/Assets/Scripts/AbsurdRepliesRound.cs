using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace AbsurdReplies
{
    public class AbsurdRepliesRound : NetworkBehaviour
    {
        public delegate void RoundFinishedDelegate();
        public event RoundFinishedDelegate onRoundFinished;

        [SerializeField] private Dice _dice;
        
        private DateTime? _startTime;
        private bool _finished;
        private QuestionCategory _questionCategory;

        private int DurationSeconds => GameSettings.Instance.RoundTimeSeconds;
        private bool Started => _startTime.HasValue;
        private bool Finished => _finished;

        private async void Awake()
        {
            DependencyValidator.ValidateDependency(_dice, nameof(_dice), nameof(AbsurdRepliesRound));
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

        public async Task StartRound()
        {
            if (!isServer)
            {
                Destroy(gameObject);
            }

            await PickCategory();
        }

        private async Task PickCategory()
        {
            const int diceToRoll = 6;
            var roll = await _dice.RollDice(diceToRoll);
            
            if (Enum.IsDefined(typeof(QuestionCategory), roll))
                _questionCategory = (QuestionCategory) roll;
            else 
                _questionCategory = QuestionCategory.Unknown;¡
        }

        private Task StartTimer()
        {
            _startTime = DateTime.UtcNow;
            return Task.CompletedTask;
        }
    }
}