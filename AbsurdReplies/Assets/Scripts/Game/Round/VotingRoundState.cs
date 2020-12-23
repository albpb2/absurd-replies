using System.Threading.Tasks;
using AbsurdReplies.Dependencies;

namespace AbsurdReplies.Game.Round
{
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
            await _gameViewsManager.DisplayVotingView();
            await round.StartTimer(GameSettings.Instance.RoundTimeSeconds);
            return this;
        }

        public async Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            await round.UpdateRemainingTime();

            if (round.RemainingSeconds <= 0)
            {
                FinishStage();
            
                await round.PlayNewRound();

                return await _waitingToStartRoundState.EnterState(round);
            }

            return this;
        }
        
        private void FinishStage()
        {
            _gameViewsManager.HideAllViews();
        }
    }
}