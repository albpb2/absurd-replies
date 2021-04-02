using System.Threading.Tasks;
using AbsurdReplies.Exceptions;
using AbsurdReplies.Infrastructure;

namespace AbsurdReplies.Game.Round
{
    public class RoundStateLogger : IRoundState
    {
        private readonly IRoundState _actualState;
        private readonly ILogger _logger;

        public RoundStateLogger(IRoundState actualState, ILogger logger)
        {
            _actualState = actualState ?? throw ExceptionBecause.MissingDependency(nameof(actualState));
            _logger = logger ?? throw ExceptionBecause.MissingDependency(nameof(logger));
        }
        
        public Task<IRoundState> EnterState(AbsurdRepliesRound round)
        {
            throw new System.NotImplementedException();
        }

        public Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            throw new System.NotImplementedException();
        }
    }
}