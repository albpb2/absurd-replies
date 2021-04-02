using System.IO;
using System.Runtime.CompilerServices;

namespace AbsurdReplies.Exceptions
{
    public static class ExceptionBecause
    {
        public static GameException MissingDependency(string dependencyName, [CallerFilePath] string whereName = "") => new GameException(
            ExceptionCodes.MissingDependency,
            $"Missing dependency: {dependencyName} not assigned on {Path.GetFileNameWithoutExtension(whereName)}");
        
        public static GameException GameIdMissing() => new GameException(
            ExceptionCodes.GameIdMissing,
            "The game id is missing");
    }
}