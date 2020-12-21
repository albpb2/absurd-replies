using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using Zenject;

namespace AbsurdReplies
{
    public class GameQuestionsProvider : NetworkBehaviour
    {
        private QuestionsProvider _questionsProvider;

        private Dictionary<QuestionCategory, List<Question>> _questionsByCategory;
        private Dictionary<QuestionCategory, List<int>> _unusedQuestionsByCategory;

        [Inject]
        public void InitializeDependencies(QuestionsProvider questionsProvider)
        {
            _questionsProvider = questionsProvider;

            _questionsByCategory = _questionsProvider.QuestionsByCategory;
        }

        private async void Awake()
        {
            _unusedQuestionsByCategory = new Dictionary<QuestionCategory, List<int>>();
            foreach (var questionCategory in _questionsByCategory.Keys)
            {
                InitializeUnusedQuestions(questionCategory);
            }
        }

        public Task<Question> GetQuestion(QuestionCategory questionCategory)
        {
            if (!_questionsByCategory.ContainsKey(questionCategory))
            {
                Debug.LogError($"No questions found for category {questionCategory}");
                return null;
            }

            var questionIndex = _unusedQuestionsByCategory[questionCategory].GetRandomElement();
            
            _unusedQuestionsByCategory[questionCategory].Remove(questionIndex);
            if (!_unusedQuestionsByCategory[questionCategory].Any())
                InitializeUnusedQuestions(questionCategory);

            return Task.FromResult(_questionsByCategory[questionCategory][questionIndex]);
        }

        private void InitializeUnusedQuestions(QuestionCategory questionCategory)
        {
            _unusedQuestionsByCategory[questionCategory] =
                Enumerable.Range(0, _questionsByCategory[questionCategory].Count).ToList();
        }
    }
}