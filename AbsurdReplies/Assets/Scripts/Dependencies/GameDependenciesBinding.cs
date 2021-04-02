using AbsurdReplies.Game.Round;
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
            Container.Bind<VotingView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<VotingProcess>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RoundStates>().ToSelf().AsSingle();
            Container.Bind<QuestionAndAnswerRoundState>().ToSelf().AsSingle();
            Container.Bind<WaitingToStartRoundState>().ToSelf().AsSingle();
            Container.Bind<VotingRoundState>().ToSelf().AsSingle();
            Container.Bind<VotingResultsRoundState>().ToSelf().AsSingle();
        }
    }
}