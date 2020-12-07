namespace AbsurdReplies.Exceptions
{
    public static class ExceptionBecause
    {
        public static GameException MissingDependency(string dependencyName, string whereName) => new GameException(
            ExceptionCodes.MissingDependency,
            $"Missing dependency: {dependencyName} not assigned on {whereName}");
        
        public static GameException GameIdMissing() => new GameException(
            ExceptionCodes.GameIdMissing,
            "The game id is missing");
    }
}