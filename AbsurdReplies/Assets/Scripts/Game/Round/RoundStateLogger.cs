using System.Threading.Tasks;
using AbsurdReplies.Exceptions;
using UnityEngine;
using ILogger = AbsurdReplies.Infrastructure.ILogger;

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
            Debug.Log($"Entering state {_actualState.GetType().Name}");
            return _actualState.EnterState(round);
        }

        public Task<IRoundState> Update(AbsurdRepliesRound round)
        {
            return _actualState.Update(round);
        }
    }
}