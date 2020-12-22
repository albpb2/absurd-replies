using System.Threading.Tasks;
using AbsurdReplies.Dependencies;

namespace AbsurdReplies.Game.Round
{
    public class QuestionAndAnswerRoundState : IRoundState
    {
        public const string Id = nameof(QuestionAndAnswerRoundState);

        private VotingRoundState _votingRoundState;
        private GameViewsManager _gameViewsManager;
        
        public QuestionAndAnswerRoundState(VotingRoundState votingRoundState, GameViewsManager gameViewsManager)
        {
            _votingRoundState = votingRoundState;
            _gameViewsManager = gameViewsManager;
            
            DependencyValidator.ValidateDependency(_votingRoundState, nameof(_votingRoundState), nameof(QuestionAndAnswerRoundState));
            DependencyValidator.ValidateDependency(_gameViewsManager, nameof(_gameViewsManager), nameof(QuestionAndAnswerRoundState));
        }

        public Task<IRoundState> EnterState(AbsurdRepliesRound round)
        {
            _gameViewsManager.DisplayQuestionAndAnswerViews(round.Question);
            round.StartTimer();
            return Task.FromResult(this as IRoundState);
        }

        public Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            round.UpdateRemainingTime();

            if (round.RemainingSeconds <= 0)
            {
                FinishStage();
                return _votingRoundState.EnterState(round);
            }

            return Task.FromResult(this as IRoundState);
        }
        
        private void FinishStage()
        {
            _gameViewsManager.HideAllViews();
        }
    }
}