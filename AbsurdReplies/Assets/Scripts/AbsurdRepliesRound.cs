using System;
using System.Threading.Tasks;
using Mirror;

namespace AbsurdReplies
{
    public class AbsurdRepliesRound : NetworkBehaviour
    {
        public delegate void RoundFinishedDelegate();
        public event RoundFinishedDelegate onRoundFinished;
        
        private DateTime? _startTime;
        private bool _finished;

        private int DurationSeconds => GameSettings.Instance.RoundTimeSeconds;
        private bool Started => _startTime.HasValue;
        private bool Finished => _finished;

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

        public Task StartRound()
        {
            if (!isServer)
            {
                Destroy(gameObject);
            }
            
            _startTime = DateTime.UtcNow;
            
            return Task.CompletedTask;
        }
    }
}