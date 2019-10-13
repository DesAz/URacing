using UnityEngine;
using URacing.Checkpoints;
using URacing.Controls;
using URacing.UI;

namespace URacing
{
    public class Gameplay : MonoBehaviour
    {
        [SerializeField] private CheckpointsController _checkpointsController = default;
        [SerializeField] private CameraController _cameraController = default;
        [SerializeField] private GhostController _ghostController = default;

        [SerializeField] private Transform _startingPoint = default;
        [SerializeField] private AvailableVehicles _vehicles = default;
        [SerializeField] private DirectionArrow _directionArrowTemplate = default;

        [SerializeField] private UIController _uiController = default;

        private DirectionArrow _directionArrow;
        private PlayerCarController _playerCar;

        private bool _gameFinished;
        private float _timer;

        private void Awake()
        {
            _playerCar = Instantiate(_vehicles.SelectedCarController, _startingPoint);

            _checkpointsController.Init(_cameraController, OnCheckpointPassed, OnCheckpointGameFinished);
            _cameraController.AddTarget(_playerCar);

            _ghostController.Init(_playerCar);
            _uiController.Init(_playerCar);

            _directionArrow = Instantiate(_directionArrowTemplate);
            _directionArrow.Init(_playerCar);

            UpdateDirectionArrow();
        }

        private void Update()
        {
            if (_gameFinished)
                return;

            _timer += Time.deltaTime;

            _checkpointsController.UpdateTimer(_timer);
            _uiController.UpdateTimer(_timer);
        }

        private void UpdateDirectionArrow()
        {
            _directionArrow.SetFollowTarget(_checkpointsController.NextCheckpoint);
        }

        private void OnCheckpointPassed(Checkpoint checkpoint)
        {
            UpdateDirectionArrow();

            _uiController.OnCheckpointPassed(checkpoint);
        }

        private void OnCheckpointGameFinished(int starsCount)
        {
            _gameFinished = true;

            _playerCar.Dispose();

            _uiController.OnCheckpointGameFinished(starsCount);

            var savedScore = new SavedScore(starsCount, _timer);

            var sceneKey = $"{gameObject.scene.name} Score";
            var savedCount = JsonUtility.FromJson<SavedScore>(PlayerPrefs.GetString(sceneKey))
                             ?? new SavedScore();

            var bestInStars = starsCount > savedCount.StarsCount;
            var bestInTime = _timer < savedCount.Time;

            var recordTime = bestInStars || bestInTime;

            if (recordTime)
                PlayerPrefs.SetString(sceneKey, JsonUtility.ToJson(savedScore));

            _ghostController.GameFinished(recordTime);
        }
    }
}