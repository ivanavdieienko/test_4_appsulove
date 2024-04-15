using System;
using UnityEngine;
using Zenject;

namespace AppsULove.TestGame
{
    public class MainInstaller : MonoInstaller
    {
        [Inject]
        private Settings _settings = null;

        public Settings GameSettings => _settings;


        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<SaveDataSignal>();
            Container.DeclareSignal<SquareDieSignal>();
            Container.DeclareSignal<UpdateDistanceSignal>();
            Container.BindSignal<SaveDataSignal>().ToMethod<SaveDataCommand>(x => x.Execute).FromNew();
            Container.BindSignal<SquareDieSignal>().ToMethod<UpdateScoreCommand>(x => x.Execute).FromNew();
            Container.BindSignal<UpdateDistanceSignal>().ToMethod<UpdateDistanceCommand>(x => x.Execute).FromNew();

            Container.Bind<GameDataModel>().AsSingle();
            Container.BindInterfacesTo<UiView>().FromComponentInHierarchy().AsCached();
            Container.BindInterfacesTo<SquareSpawner>().AsSingle();

            Container.BindFactory<Square, Square.Factory>()
                // We could just use FromMonoPoolableMemoryPool here instead, but
                // for IL2CPP to work we need our pool class to be used explicitly here
                .FromPoolableMemoryPool<Square, SquarePool>(poolBinder => poolBinder
                    // Spawn 5 enemies right off the bat so that we don't incur spikes at runtime
                    .WithInitialSize(_settings.MaxSquareCount)
                    .FromComponentInNewPrefab(_settings.SquarePrefab)
                    // Place each enemy under an Enemies game object at the root of scene hierarchy
                    .UnderTransformGroup("Squares"));
        }

        [Serializable]
        public class Settings
        {
            public float PlayerSpeed = 1f;
            public int MaxSquareCount = 5;
            public GameObject SquarePrefab;
        }

        private class SquarePool : MonoPoolableMemoryPool<IMemoryPool, Square>
        {
        }
    }
}