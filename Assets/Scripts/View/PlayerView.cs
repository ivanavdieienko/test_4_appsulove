using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace AppsULove.TestGame
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private CircleCollider2D _body;

        [Inject]
        private readonly SignalBus _signalBus;

        private IDisposable _moveSubscription;

        private float _speed = 0f;

        [Inject]
        public void Construct(MainInstaller.Settings settings)
        {
            _speed = settings.PlayerSpeed;
        }

        private void Start()
        {
            // stop on touch the circle
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
                .Where(touchPosition => _body.OverlapPoint(touchPosition))
                .Subscribe(_ => EndMovement())
                .AddTo(this);

            // start after touch the screen
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
                .Where(touchPosition => Vector3.Distance(transform.position, touchPosition) >= _body.radius)
                .Subscribe(touchPosition => TryMove(touchPosition), EndMovement)
                .AddTo(this);
        }

        private void OnApplicationQuit()
        {
            _signalBus.Fire(new SaveDataSignal());
        }

        private void TryMove(Vector3 targetPosition)
        {
            EndMovement();

            var canMove = Vector3.Distance(transform.position, targetPosition) >= _body.radius;
            if (canMove)
            {
                targetPosition.z = transform.position.z; //fix slip to camera z: -10

                var journeyLength = Vector3.Distance(transform.position, targetPosition) / _speed;
                var startTime = Time.time;

                _moveSubscription = Observable.EveryUpdate()
                    .Select(_ => (Time.time - startTime) / journeyLength)
                    .TakeWhile(t => t < 1.0f)
                    .Subscribe(t =>
                    {
                        var smoothedT = Mathf.SmoothStep(0.0f, 1.0f, t);
                        var oldPosition = transform.position;
                        var newPosition = Vector3.Lerp(oldPosition, targetPosition, smoothedT);

                        transform.position = newPosition;

                        var distance = Math.Abs((newPosition - oldPosition).magnitude);

                        _signalBus.Fire(new UpdateDistanceSignal() { Distance = distance });
;                    }, EndMovement);
            }
        }

        private void EndMovement()
        {
            if (_moveSubscription != null)
            {
                _moveSubscription.Dispose();
                _moveSubscription = null;
            }
        }
    }
}