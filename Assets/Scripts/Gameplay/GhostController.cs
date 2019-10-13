using System.Collections.Generic;
using UnityEngine;
using URacing.Controls;

namespace URacing
{
    public class GhostController : MonoBehaviour
    {
        public class PositionAndRotation
        {
            public Vector3 Position;
            public Quaternion Rotation;

            public PositionAndRotation(Vector3 position, Quaternion rotation)
            {
                Position = position;
                Rotation = rotation;
            }
        }

        [SerializeField] private Ghost _ghostTemplate = default;

        private static readonly Dictionary<string, HashSet<PositionAndRotation>> PositionsCache =
            new Dictionary<string, HashSet<PositionAndRotation>>();

        private readonly HashSet<PositionAndRotation> _currentPositions = new HashSet<PositionAndRotation>();

        private PlayerCarController _playerCarController;

        private Ghost _ghost;
        private bool _recording;

        public void Init(PlayerCarController playerCarController)
        {
            _playerCarController = playerCarController;
            _recording = true;

            var sceneName = gameObject.scene.name;

            if (!PositionsCache.ContainsKey(sceneName))
                return;

            var loadedPositions = PositionsCache[sceneName];

            if (loadedPositions.Count > 0)
                SetGhostPositions(loadedPositions);
        }

        private void Update()
        {
            if (_playerCarController == null)
                return;

            if (!_recording)
                return;

            var playerTransform = _playerCarController.transform;
            _currentPositions.Add(new PositionAndRotation(playerTransform.position, playerTransform.rotation));
        }

        public void GameFinished(bool saveToCache)
        {
            _recording = false;

            if (!saveToCache)
                return;

            PositionsCache[gameObject.scene.name] = _currentPositions;
        }

        private async void SetGhostPositions(IEnumerable<PositionAndRotation> cachedData)
        {
            _ghost = Instantiate(_ghostTemplate);
            _ghost.Init();

            foreach (var setting in cachedData)
            {
                if (_ghost == null)
                    return;

                var ghostTransform = _ghost.transform;

                ghostTransform.position = setting.Position;
                ghostTransform.rotation = setting.Rotation;

                await new WaitForSeconds(0);
            }
        }
    }
}