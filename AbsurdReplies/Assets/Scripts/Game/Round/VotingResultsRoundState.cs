using System.Collections;
using System.Threading.Tasks;
using AbsurdReplies.Exceptions;
using UnityEngine;

namespace AbsurdReplies.Game.Round
{
    public class VotingResultsRoundState : IRoundState
    {
        private const int StateSeconds = 5;
        
        private WaitingToStartRoundState _waitingToStartRoundState;

        private bool _moveToNextState;

        public VotingResultsRoundState(WaitingToStartRoundState waitingToStartRoundState)
        {
            _waitingToStartRoundState = waitingToStartRoundState ?? throw ExceptionBecause.MissingDependency(nameof(waitingToStartRoundState));
        }

        public Task<IRoundState> EnterState(AbsurdRepliesRound round)
        {
            _moveToNextState = false;
            round.StartCoroutine(MoveToNextRound());
            return Task.FromResult(this as IRoundState);
        }

        public async Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            if (_moveToNextState)
            {
                await round.PlayNewRound();
                return await _waitingToStartRoundState.EnterState(round);
            }
            
            return this;
        }

        private IEnumerator MoveToNextRound()
        {
            yield return new WaitForSeconds(StateSeconds);

            _moveToNextState = true;
        }
    }
}