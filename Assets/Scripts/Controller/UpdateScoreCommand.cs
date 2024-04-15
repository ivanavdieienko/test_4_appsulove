using Zenject;

namespace AppsULove.TestGame
{
    internal class UpdateScoreCommand
    {
        [Inject]
        private readonly GameDataModel _gameDataModel;

        public void Execute()
        {
            _gameDataModel.UpdateScore();
        }
    }
}
