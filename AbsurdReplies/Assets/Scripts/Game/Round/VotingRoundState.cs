using System.Threading.Tasks;
using AbsurdReplies.Exceptions;
using AbsurdReplies.Infrastructure;

namespace AbsurdReplies.Game.Round
{
    public class VotingRoundState : IRoundState
    {
        private IRoundState _votingResultsRoundState;
        private GameViewsManager _gameViewsManager;
        private VotingProcess _votingProcess;

        public VotingRoundState(
            VotingResultsRoundState votingResultsRoundState, 
            GameViewsManager gameViewsManager,
            VotingProcess votingProcess,
            ILogger logger)
        {
            _votingResultsRoundState = new RoundStateLogger(votingResultsRoundState, logger) ?? throw ExceptionBecause.MissingDependency(nameof(votingResultsRoundState));
            _gameViewsManager = gameViewsManager ?? throw ExceptionBecause.MissingDependency(nameof(gameViewsManager));
            _votingProcess = votingProcess ?? throw ExceptionBecause.MissingDependency(nameof(votingProcess));
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