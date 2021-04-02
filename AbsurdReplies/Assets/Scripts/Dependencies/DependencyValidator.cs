using System.Runtime.CompilerServices;
using AbsurdReplies.Exceptions;

namespace AbsurdReplies.Dependencies
{
    public static class DependencyValidator
    {
        public static void ValidateDependency<T>(T dependency, string dependencyName, [CallerFilePath] string whereName = "") where T : class
        {
            if (dependency == null)
            {
                throw ExceptionBecause.MissingDependency(dependencyName, whereName);
            }
        }
    }
}