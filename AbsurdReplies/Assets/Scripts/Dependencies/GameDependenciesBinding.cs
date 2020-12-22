using Zenject;

namespace AbsurdReplies.Dependencies
{
    public class GameDependenciesBinding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AbsurdRepliesGame>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AbsurdRepliesRound>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RoundReplies>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameViewsManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameQuestionsProvider>().FromComponentInHierarchy().AsSingle();
        }
    }
}