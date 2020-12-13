﻿using System;
using System.Threading.Tasks;

namespace AbsurdReplies
{
    public class QuestionCategorySelector
    {
        private Dice _dice;

        public QuestionCategorySelector(Dice dice)
        {
            _dice = dice;
            DependencyValidator.ValidateDependency(_dice, nameof(_dice), nameof(QuestionCategorySelector));
        }

        public async Task<QuestionCategory> SelectRandomQuestionCategory()
        {
            const int diceToRoll = 6;
            var roll = await _dice.RollDice(diceToRoll);
            
            if (Enum.IsDefined(typeof(QuestionCategory), roll))
                return (QuestionCategory) roll;
            
            return QuestionCategory.Unknown;
        }
    }
}