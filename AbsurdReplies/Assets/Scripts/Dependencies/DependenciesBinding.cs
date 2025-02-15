﻿using AbsurdReplies.Infrastructure;
using Zenject;

namespace AbsurdReplies.Dependencies
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
            Container.Bind<QuestionParser>().ToSelf().AsCached();
            Container.Bind<QuestionsProvider>().ToSelf().AsCached();
            Container.Bind<ILogger>().To<Logger>().AsCached();
        }
    }
}