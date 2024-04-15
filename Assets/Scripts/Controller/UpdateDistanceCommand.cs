using Zenject;

namespace AppsULove.TestGame
{
    internal class UpdateDistanceCommand
    {
        [Inject]
        private readonly GameDataModel _gameDataModel;

        public void Execute(UpdateDistanceSignal signal)
        {
            _gameDataModel.PlayerMoved(signal.Distance);
        }
    }
}
