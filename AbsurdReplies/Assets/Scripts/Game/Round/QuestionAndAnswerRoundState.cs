using System.Threading.Tasks;
using AbsurdReplies.Exceptions;

namespace AbsurdReplies.Game.Round
{
    public class QuestionAndAnswerRoundState : IRoundState
    {
        public const string Id = nameof(QuestionAndAnswerRoundState);

        private VotingRoundState _votingRoundState;
        private GameViewsManager _gameViewsManager;
        
        public QuestionAndAnswerRoundState(VotingRoundState votingRoundState, GameViewsManager gameViewsManager)
        {
            _votingRoundState = votingRoundState ?? throw ExceptionBecause.MissingDependency(nameof(votingRoundState));
            _gameViewsManager = gameViewsManager ?? throw ExceptionBecause.MissingDependency(nameof(gameViewsManager));
        }

        public async Task<IRoundState> EnterState(AbsurdRepliesRound round)
        {
            _gameViewsManager.DisplayQuestionAndAnswerViews(round.Question);
            await round.StartTimer(GameSettings.Instance.RoundTimeSeconds);
            return this;
        }

        public async Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            await round.UpdateRemainingTime();

            if (round.RemainingSeconds <= 0)
            {
                FinishStage();
                return await _votingRoundState.EnterState(round);
            }

            return this;
        }
        
        private void FinishStage()
        {
            _gameViewsManager.HideAllViews();
        }
    }
}