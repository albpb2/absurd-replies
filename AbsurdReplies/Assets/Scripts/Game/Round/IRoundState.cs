using System.Threading.Tasks;

namespace AbsurdReplies.Game.Round
{
    public interface IRoundState
    {
        Task<IRoundState> EnterState(AbsurdRepliesRound round); 
        Task<IRoundState> Update(AbsurdRepliesRound round);
    }
}