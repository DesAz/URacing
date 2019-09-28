using System;
using UnityEngine;
using URacing.Controls;

namespace URacing.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        private Action<Checkpoint, ICarController> _onCheckpointEnter;

        public void Init(Action<Checkpoint, ICarController> onCheckpointEnter)
        {
            _onCheckpointEnter = onCheckpointEnter;
        }

        private void OnTriggerEnter(Collider other)
        {
            var carController = other.GetComponent<ICarController>();

            if (carController != null)
                _onCheckpointEnter?.Invoke(this, carController);
        }

        private void OnTriggerExit(Collider other)
        {
//            var carController = other.GetComponent<ICarController>();

//            if (carController != null)
//                _onCheckpointVisited?.Invoke(carController);
        }

        public void PassByPlayer()
        {
            gameObject.SetActive(false);
        }
    }
}