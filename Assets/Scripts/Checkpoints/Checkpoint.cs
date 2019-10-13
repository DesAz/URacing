using System;
using UnityEngine;
using URacing.Controls;

namespace URacing.Checkpoints
{
    public class Checkpoint : MonoBehaviour, ICameraTarget
    {
        [SerializeField] private MeshRenderer _meshRenderer = default;

        private Action<Checkpoint, ICarController> _onCheckpointEnter;

        public Transform Follow => transform;
        public bool ForceShow => false;

        public bool Passed { get; private set; }
        public float DesiredTime { get; private set; }

        public void Init(float checkpointTime, Action<Checkpoint, ICarController> onCheckpointEnter)
        {
            DesiredTime = checkpointTime;
            _onCheckpointEnter = onCheckpointEnter;
        }

        public void SetAsNextCheckpoint()
        {
            _meshRenderer.material.color = Color.green;
        }

        private void OnTriggerEnter(Collider other)
        {
            var carController = other.GetComponent<ICarController>();

            if (carController != null)
                _onCheckpointEnter?.Invoke(this, carController);
        }

        public void PassByPlayer()
        {
            Passed = true;

            gameObject.SetActive(false);
        }
    }
}