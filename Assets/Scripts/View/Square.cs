using System;
using UnityEngine;
using Zenject;

namespace AppsULove.TestGame
{
    public class Square : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        IMemoryPool _pool;

        [Inject]
        private SignalBus _signalBus;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _signalBus.Fire(new SquareDieSignal());
                _pool.Despawn(this);
            }
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public class Factory : PlaceholderFactory<Square>
        {
        }
    }
}
