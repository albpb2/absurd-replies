using System.Threading.Tasks;

namespace AbsurdReplies.Game.Round
{
    public class WaitingToStartRoundState : IRoundState
    {
        public const string Id = nameof(WaitingToStartRoundState);

        public Task<IRoundState> EnterState(AbsurdRepliesRound round) => Task.FromResult(this as IRoundState);

        public Task<IRoundState> Update(AbsurdRepliesRound round) => Task.FromResult(this as IRoundState);
    }
}