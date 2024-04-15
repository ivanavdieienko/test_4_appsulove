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

        private void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();

            // Generate a random color in HSV, then convert it to RGB
            spriteRenderer.color = Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f); ;
        }

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
