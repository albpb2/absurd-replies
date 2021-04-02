using System;
using System.Threading.Tasks;
using AbsurdReplies.Dependencies;

namespace AbsurdReplies
{
    public class QuestionCategorySelector
    {
        private Dice _dice;

        public QuestionCategorySelector(Dice dice)
        {
            _dice = dice;
            DependencyValidator.ValidateDependency(_dice, nameof(_dice));
        }

        public async Task<QuestionCategory> SelectRandomQuestionCategory()
        {
            if (DebugOptions.Instance.ForceUnknownCategory)
                return QuestionCategory.Unknown;
            
            const int diceToRoll = 6;
            var roll = await _dice.RollDice(diceToRoll);
            
            if (Enum.IsDefined(typeof(QuestionCategory), roll))
                return (QuestionCategory) roll;
            
            return QuestionCategory.Unknown;
        }
    }
}