using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AbsurdReplies
{
    public class QuestionParser
    {
        public Dictionary<QuestionCategory, List<Question>> ParseQuestions(TextAsset textAsset)
        {
            var questionsByCategory = new Dictionary<QuestionCategory, List<Question>>();

            var lines = textAsset.text.Split(new []{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();

            var currentCategory = QuestionCategory.Unknown;
            var skipNextLine = false;

            for (var i = 0; i < lines.Count; i++)
            {
                if (skipNextLine)
                {
                    skipNextLine = false;
                    continue;
                }
                
                var line = lines[i];

                const string categoryLineStart = "Category:";
                if (line.StartsWith(categoryLineStart))
                {
                    currentCategory = (QuestionCategory)Enum.Parse(typeof(QuestionCategory), line.Replace(categoryLineStart, string.Empty));
                }
                else if (line.Length > 1)
                {
                    ParseQuestion(currentCategory, line, lines[i + 1], questionsByCategory);
                    skipNextLine = true;
                }
            }

            return questionsByCategory;
        }

        private static void ParseQuestion(QuestionCategory currentCategory, string heading, string answer,
            IDictionary<QuestionCategory, List<Question>> questionsByCategory)
        {
            var question = new Question
            {
                Category = currentCategory,
                Heading = heading,
                Answer = answer,
            };

            if (!questionsByCategory.ContainsKey(currentCategory))
            {
                questionsByCategory[currentCategory] = new List<Question>();
            }

            questionsByCategory[currentCategory].Add(question);
        }
    }
}