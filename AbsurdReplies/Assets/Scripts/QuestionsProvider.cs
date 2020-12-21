using System.Collections.Generic;
using SPStudios.Tools;
using UnityEngine;

namespace AbsurdReplies
{
    public class QuestionsProvider : Singleton<QuestionsProvider>
    {
        private const string QuestionsPath = "Questions";

        private QuestionParser _questionParser;
        
        private Dictionary<QuestionCategory, List<Question>> _questionsByCategory;

        public Dictionary<QuestionCategory, List<Question>> QuestionsByCategory => _questionsByCategory;
        
        protected override void OnInit()
        {
            base.OnInit();

            _questionParser = new QuestionParser(); // Can't use zenject with singletons (need to investigate)
   
            var questionsText = Resources.Load(QuestionsPath) as TextAsset;
            _questionsByCategory = _questionParser.ParseQuestions(questionsText);
        }
    }
}