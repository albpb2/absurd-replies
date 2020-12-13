using Zenject;

namespace AbsurdReplies
{
    public class DependenciesBinding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EnvironmentProvider>().ToSelf().AsCached();
            Container.Bind<ServerUrlProvider>().ToSelf().AsCached();
            Container.Bind<GameCodeRetriever>().ToSelf().AsCached();
            Container.Bind<Dice>().ToSelf().AsCached();
            Container.Bind<QuestionCategorySelector>().ToSelf().AsCached();
        }
    }
}