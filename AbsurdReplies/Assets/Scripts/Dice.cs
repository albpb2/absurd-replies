using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace AbsurdReplies
{
    public class Dice : MonoBehaviour
    {
        private const int MinDiceValue = 1;
        
        private Random _random;

        private async void Awake()
        {
            _random = new Random();
        }

        public Task<int> RollDice(int max) => Task.FromResult(_random.Next(MinDiceValue, max));
    }
}