using System.Collections;
using System.Threading.Tasks;
using AbsurdReplies.Dependencies;
using UnityEngine;

namespace AbsurdReplies.Game.Round
{
    /// <summary>
    /// Unimplemented.
    /// ATM this contains some dirty code to end the round in 5 seconds just for demo purposes.
    /// </summary>
    public class VotingRoundState : IRoundState
    {
        private WaitingToStartRoundState _waitingToStartRoundState;
        private GameViewsManager _gameViewsManager;

        private bool _roundFinished;

        public VotingRoundState(WaitingToStartRoundState waitingToStartRoundState, GameViewsManager gameViewsManager)
        {
            _waitingToStartRoundState = waitingToStartRoundState;
            _gameViewsManager = gameViewsManager;
            
            DependencyValidator.ValidateDependency(_waitingToStartRoundState, nameof(_waitingToStartRoundState), nameof(VotingRoundState));
            DependencyValidator.ValidateDependency(_gameViewsManager, nameof(_gameViewsManager), nameof(VotingRoundState));
        }

        public async Task<IRoundState> EnterState(AbsurdRepliesRound round)
        {
            _roundFinished = false;
            await _gameViewsManager.DisplayVotingView();
            round.StartCoroutine(FinishRoundIn5Seconds());
            return this;
        }

        public async Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            if (!_roundFinished)
                return this;
            
            await round.FinishRound();

            _gameViewsManager.HideAllViews();
            
            await round.PlayNewRound();

            return await _waitingToStartRoundState.EnterState(round);
        }

        private IEnumerator FinishRoundIn5Seconds()
        {
            yield return new WaitForSeconds(5);
            _roundFinished = true;
        }
    }
}