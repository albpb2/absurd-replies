using System.Threading.Tasks;
using AbsurdReplies.Dependencies;

namespace AbsurdReplies.Game.Round
{
    public class VotingRoundState : IRoundState
    {
        private VotingResultsRoundState _votingResultsRoundState;
        private GameViewsManager _gameViewsManager;
        private VotingProcess _votingProcess;

        public VotingRoundState(
            VotingResultsRoundState votingResultsRoundState, 
            GameViewsManager gameViewsManager,
            VotingProcess votingProcess)
        {
            _votingResultsRoundState = votingResultsRoundState;
            _gameViewsManager = gameViewsManager;
            _votingProcess = votingProcess;
            
            DependencyValidator.ValidateDependency(_votingResultsRoundState, nameof(_votingResultsRoundState), nameof(VotingRoundState));
            DependencyValidator.ValidateDependency(_gameViewsManager, nameof(_gameViewsManager), nameof(VotingRoundState));
            DependencyValidator.ValidateDependency(_votingProcess, nameof(_votingProcess), nameof(VotingRoundState));
        }

        public async Task<IRoundState> EnterState(AbsurdRepliesRound round)
        {
            await _gameViewsManager.DisplayVotingView();
            await _votingProcess.StartProcess();
            await round.StartTimer(GameSettings.Instance.RoundTimeSeconds);
            return this;
        }

        public async Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            await round.UpdateRemainingTime();

            if (round.RemainingSeconds <= 0)
            {
                FinishStage();

                await round.FinishRound();
                await round.PlayNewRound();

                return await _votingResultsRoundState.EnterState(round);
            }

            return this;
        }
        
        private void FinishStage()
        {
            _gameViewsManager.HideAllViews();
        }
    }
}