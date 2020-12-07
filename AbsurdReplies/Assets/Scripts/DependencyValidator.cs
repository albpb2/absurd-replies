using AbsurdReplies.Exceptions;

namespace AbsurdReplies
{
    public static class DependencyValidator
    {
        public static void ValidateDependency<T>(T dependency, string dependencyName, string whereName) where T : class
        {
            if (dependency == null)
            {
                throw ExceptionBecause.MissingDependency(dependencyName, whereName);
            }
        }
    }
}