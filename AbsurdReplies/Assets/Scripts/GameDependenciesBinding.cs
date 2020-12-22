using Zenject;

namespace AbsurdReplies
{
    public class GameDependenciesBinding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AbsurdRepliesRound>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RoundReplies>().FromComponentInHierarchy().AsSingle();
        }
    }
}