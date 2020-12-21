namespace AbsurdReplies
{
    public struct NetworkQuestion
    {
        public QuestionCategory Category;
        
        public string Heading;
        
        public string Answer;

        public static NetworkQuestion From(Question question) => new NetworkQuestion
        {
            Category = question.Category,
            Heading = question.Heading,
            Answer = question.Answer,
        };
    }
}