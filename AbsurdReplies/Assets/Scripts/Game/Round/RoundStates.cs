using System.Collections.Generic;

namespace AbsurdReplies.Game.Round
{
    public class RoundStates
    {
        private Dictionary<string, IRoundState> _roundStatesById;

        public RoundStates(
            QuestionAndAnswerRoundState questionAndAnswerRoundState,
            WaitingToStartRoundState waitingToStartRoundState)
        {
            _roundStatesById[QuestionAndAnswerRoundState.Id] = questionAndAnswerRoundState;
            _roundStatesById[WaitingToStartRoundState.Id] = waitingToStartRoundState;
        }

        public IRoundState this[string id] => _roundStatesById[id];
    }
}