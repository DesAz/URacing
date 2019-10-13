using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using URacing.Controls;

namespace URacing.Checkpoints
{
    public class CheckpointsController : MonoBehaviour
    {
        [Serializable]
        private class RoundTimeSettings
        {
            public int StarsCount = default;
            public float TimeAmount = default;
        }

        private const int CHECKPOINTS_TO_SHOW_COUNT = 2;

        [SerializeField] private RoundTimeSettings[] _timeSettings = default;
        [SerializeField] private Checkpoint[] _checkpoints = default;

        private CameraController _cameraController;
        private Action<Checkpoint> _onCheckpointPassed;
        private Action<int> _onCheckpointGameFinished;

        private float _timer;

        private readonly Dictionary<float, int> _timeCounters = new Dictionary<float, int>();
        private readonly List<Checkpoint> _availableCheckpoints = new List<Checkpoint>();

        public Checkpoint NextCheckpoint { get; private set; }

        public void Init(CameraController cameraController, Action<Checkpoint> onCheckpointPassed,
            Action<int> onCheckpointGameFinished)
        {
            _cameraController = cameraController;
            _onCheckpointPassed = onCheckpointPassed;
            _onCheckpointGameFinished = onCheckpointGameFinished;

            var bestTime = float.MaxValue;

            foreach (var roundTime in _timeSettings)
            {
                _timeCounters.Add(roundTime.TimeAmount, roundTime.StarsCount);

                if (roundTime.TimeAmount < bestTime)
                    bestTime = roundTime.TimeAmount;
            }

            var oneCheckpointTime = bestTime / _checkpoints.Length;

            for (var i = 0; i < _checkpoints.Length; i++)
            {
                var checkpoint = _checkpoints[i];

                checkpoint.Init(i > 0 ? oneCheckpointTime * i : oneCheckpointTime, OnCheckpointEnter);

                _availableCheckpoints.Add(checkpoint);
            }

            SetNextCheckpoint();
        }

        public void UpdateTimer(float time)
        {
            _timer = time;
        }

        private void OnCheckpointEnter(Checkpoint checkpoint, ICarController carController)
        {
            if (checkpoint != NextCheckpoint)
                return;

#if UNITY_EDITOR
            Debug.Log(carController.Name + " has visited the checkpoint " + checkpoint.gameObject.name + ", in " +
                      _timer);
#endif

            _availableCheckpoints.Remove(checkpoint);

            checkpoint.PassByPlayer();

            if (_availableCheckpoints.Count <= 0)
            {
                Debug.Log("Game over");

                var starsCount = CalculateStarsCount(_timer);
                _onCheckpointGameFinished?.Invoke(starsCount);

                Debug.Log("Stars: " + starsCount);

                return;
            }

            SetNextCheckpoint();
            _onCheckpointPassed?.Invoke(checkpoint);
        }

        private void SetNextCheckpoint()
        {
            NextCheckpoint = _availableCheckpoints[0];
            NextCheckpoint.SetAsNextCheckpoint();

            AddCheckpointsToCamera();
        }

        private void AddCheckpointsToCamera()
        {
            _cameraController.ClearTargets();

            var amountOfAddedTargets = 0;

            foreach (var checkpoint in _checkpoints.Where(checkpoint => !checkpoint.Passed))
            {
                _cameraController.AddTarget(checkpoint);

                amountOfAddedTargets++;

                if (amountOfAddedTargets > CHECKPOINTS_TO_SHOW_COUNT)
                    return;
            }
        }

        private int CalculateStarsCount(float target)
        {
            var starsAmount = 1;

            foreach (var timeCounter in _timeCounters.Where(timeCounter => target <= timeCounter.Key))
                starsAmount = timeCounter.Value;

            return starsAmount;
        }

#if UNITY_EDITOR
        public void UpdateCheckpoints(Checkpoint[] checkpoints)
        {
            _checkpoints = checkpoints;
        }
#endif
    }
}