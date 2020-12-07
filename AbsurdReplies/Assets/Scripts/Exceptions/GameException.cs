using System;

namespace AbsurdReplies.Exceptions
{
    public class GameException : Exception
    {
        public int ExceptionCode { get; private set; }
        
        public GameException(int exceptionCode, string message) : base(message)
        {
            ExceptionCode = exceptionCode;
        }
    }
}