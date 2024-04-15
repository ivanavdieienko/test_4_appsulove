using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace AppsULove.TestGame
{
    public class SquareSpawner : ITickable, IInitializable, IDisposable
    {
        private readonly Square.Factory _squareFactory;
        private readonly int _maxSquares;
        private readonly SignalBus _signalBus;

        private int _squaresCount;


        public SquareSpawner(SignalBus signalBus, Square.Factory squareFactory, MainInstaller.Settings settings)
        {
            _signalBus = signalBus;
            _squareFactory = squareFactory;
            _maxSquares = settings.MaxSquareCount;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<SquareDieSignal>(OnSquareDie);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<SquareDieSignal>(OnSquareDie);
        }

        public void Tick()
        {
            if (ShouldSpawnNewSquare())
            {
                var randomPosition = new Vector3(
                    Random.Range(0, Screen.width),
                    Random.Range(0, Screen.height),
                    0
                );

                randomPosition = Camera.main.ScreenToWorldPoint(randomPosition);
                randomPosition.z = 0;

                var square = _squareFactory.Create();
                square.transform.position = randomPosition;
                _squaresCount++;
            }
        }

        private bool ShouldSpawnNewSquare()
        {
            return _squaresCount < _maxSquares;
        }

        private void OnSquareDie()
        {
            _squaresCount--;
        }
    }
}
