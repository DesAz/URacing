using UnityEngine;
using URacing.Checkpoints;
using URacing.Controls;

namespace URacing
{
    public class Gameplay : MonoBehaviour
    {
        [SerializeField] private CheckpointsController _checkpointsController = default;

        [SerializeField] private Transform _startingPoint = default;
        [SerializeField] private PlayerCarController _playerCarTemplate = default;

        private void Awake()
        {
            var playerCar = Instantiate(_playerCarTemplate, _startingPoint);

            _checkpointsController.Init();
        }
    }
}