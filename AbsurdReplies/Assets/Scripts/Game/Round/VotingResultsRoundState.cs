using System.Collections;
using System.Threading.Tasks;
using AbsurdReplies.Dependencies;
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
            _waitingToStartRoundState = waitingToStartRoundState;

            DependencyValidator.ValidateDependency(_waitingToStartRoundState, nameof(_waitingToStartRoundState), nameof(WaitingToStartRoundState));
        }

        public Task<IRoundState> EnterState(AbsurdRepliesRound round)
        {
            _moveToNextState = false;
            round.StartCoroutine(MoveToNextRound());
            return Task.FromResult(this as IRoundState);
        }

        public Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            if (_moveToNextState)
                return _waitingToStartRoundState.EnterState(round);
            
            return Task.FromResult(this as IRoundState);
        }

        private IEnumerator MoveToNextRound()
        {
            yield return new WaitForSeconds(StateSeconds);

            _moveToNextState = true;
        }
    }
}