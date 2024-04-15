using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace AppsULove.TestGame
{
    public class UiView : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField]
        private TextMeshProUGUI _distanceUi;

        [SerializeField]
        private TextMeshProUGUI _scoreUi;

        [Inject]
        private GameDataModel _gameDataModel;

        [Inject]
        private SignalBus _signalBus;


        public void Initialize()
        {
            _signalBus.Subscribe<SquareDieSignal>(UpdateScore);
            _signalBus.Subscribe<UpdateDistanceSignal>(UpdateDistance);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<SquareDieSignal>(UpdateScore);
            _signalBus.Unsubscribe<UpdateDistanceSignal>(UpdateDistance);
        }

        private void Start()
        {
            UpdateText();
        }

        private void UpdateScore()
        {
            _scoreUi.text = $"Score: {_gameDataModel.Score} pts";
        }

        private void UpdateDistance()
        {
            _distanceUi.text = $"Distance: {_gameDataModel.Distance}";
        }

        private void UpdateText()
        {
            UpdateScore();
            UpdateDistance();
        }
    }
}
