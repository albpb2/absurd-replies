using System.Threading.Tasks;
using AbsurdReplies.Dependencies;

namespace AbsurdReplies.Game.Round
{
    public class VotingRoundState : IRoundState
    {
        private WaitingToStartRoundState _waitingToStartRoundState;

        public VotingRoundState(WaitingToStartRoundState waitingToStartRoundState)
        {
            _waitingToStartRoundState = waitingToStartRoundState;
            
            DependencyValidator.ValidateDependency(_waitingToStartRoundState, nameof(_waitingToStartRoundState), nameof(VotingRoundState));
        }
        
        public Task<IRoundState> EnterState(AbsurdRepliesRound round) => Task.FromResult(this as IRoundState);

        public async Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            await round.FinishRound();
            
            await round.PlayNewRound();

            return await _waitingToStartRoundState.EnterState(round);
        }
    }
}