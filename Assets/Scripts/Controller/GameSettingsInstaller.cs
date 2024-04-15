using UnityEngine;
using Zenject;

namespace AppsULove.TestGame
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public MainInstaller.Settings Settings;

        public override void InstallBindings()
        {
            Container.BindInstance(Settings).IfNotBound();
        }
    }
}