using System;
using System.Collections.Generic;
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

        [SerializeField] private RoundTimeSettings[] _timeSettings = default;
        [SerializeField] private Checkpoint[] _checkpoints = default;

        private float _timer;

        private Dictionary<float, int> _timeCounters = new Dictionary<float, int>();

        private List<Checkpoint> _availableCheckpoints = new List<Checkpoint>();
        private Checkpoint _nextCheckpoint;

        public void Init()
        {
            foreach (var roundTime in _timeSettings)
                _timeCounters.Add(roundTime.TimeAmount, roundTime.StarsCount);

            foreach (var checkpoint in _checkpoints)
            {
                _availableCheckpoints.Add(checkpoint);
                checkpoint.Init(OnCheckpointEnter);
            }

            _nextCheckpoint = _availableCheckpoints[0];
        }

        private void Update()
        {
            _timer += Time.deltaTime;
        }

        private void OnCheckpointEnter(Checkpoint checkpoint, ICarController carController)
        {
            if (checkpoint != _nextCheckpoint)
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

                var time = _timer;
                var starsCount = CalculateStarsCount(time);

                Debug.Log("Stars: " + starsCount);

                return;
            }

            _nextCheckpoint = _availableCheckpoints[0];
        }

        public int CalculateStarsCount(float target)
        {
            var starsAmount = 0;

            foreach (var timeCounter in _timeCounters)
            {
                if (target <= timeCounter.Key)
                    starsAmount = timeCounter.Value;
            }

            return starsAmount;
        }
    }
}