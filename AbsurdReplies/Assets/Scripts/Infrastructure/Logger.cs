using UnityEngine;

namespace AbsurdReplies.Infrastructure
{
    public class Logger : ILogger
    {
        public void LogDebug(string message)
        {
#if DEBUG || UNITY_EDITOR
            Debug.Log(message);
#endif
        }
    }
}