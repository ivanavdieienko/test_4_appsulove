using Zenject;

namespace AppsULove.TestGame
{
    internal class SaveDataCommand
    {
        [Inject]
        private readonly GameDataModel _gameDataModel;

        public void Execute()
        {
            _gameDataModel.SaveData();
        }
    }
}
