using System.Threading.Tasks;
using Random = System.Random;

namespace AbsurdReplies
{
    public class Dice
    {
        private const int MinDiceValue = 1;
        
        private Random _random;

        public Dice()
        {
            _random = new Random();
        }

        public Task<int> RollDice(int max) => Task.FromResult(_random.Next(MinDiceValue, max));
    }
}