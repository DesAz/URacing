using UnityEngine;
using System;
using System.Linq;

namespace URacing.Controls
{
    public interface ICarController
    {
        string Name { get; }
    }

    public class PlayerCarController : MonoBehaviour, ICarController, ICameraTarget, IDisposable
    {
        [Serializable]
        private enum ECarDrivingType
        {
            RearWheelDrive,
            FrontWheelDrive,
            AllWheelDrive
        }

        [Serializable]
        private class Wheel
        {
            public WheelCollider Collider = default;
            public Transform Shape = default;

            public float LocalZPosition => Collider.transform.localPosition.z;
        }

        private const KeyCode HAND_BRAKE = KeyCode.Space;

        [SerializeField] private Rigidbody _rigidbody = default;
        [SerializeField] private ECarDrivingType _carDrivingType = default;

        [SerializeField] private float _maxSteeringAngle = 30f;
        [SerializeField] private float _maxTorque = 300f;
        [SerializeField] private float _maxBrakeTorque = 30000f;

        [SerializeField] private float _criticalEngineSpeed = 5f;
        [SerializeField] private int _stepsBelowCritical = 5;
        [SerializeField] private int _stepsAboveCritical = 1;

        [SerializeField] private Wheel[] _wheels = default;

        public string Name => "PlayerController";

        public Rigidbody Rigidbody => _rigidbody;
        public Transform Follow => transform;
        public bool ForceShow => true;

        private void Update()
        {
            foreach (var wheel in _wheels)
                wheel.Collider.ConfigureVehicleSubsteps(_criticalEngineSpeed, _stepsBelowCritical, _stepsAboveCritical);

            var angle = _maxSteeringAngle * Input.GetAxis("Horizontal");
            var torque = _maxTorque * Input.GetAxis("Vertical");

            var handBrake = Input.GetKey(HAND_BRAKE) ? _maxBrakeTorque : 0f;

            foreach (var wheel in _wheels)
            {
                var wheelLocalZPosition = wheel.LocalZPosition;

                var wheelCollider = wheel.Collider;
                var wheelShape = wheel.Shape;

                if (wheelLocalZPosition > 0)
                    wheelCollider.steerAngle = angle;

                if (wheelLocalZPosition < 0)
                    wheelCollider.brakeTorque = handBrake;

                if (wheelLocalZPosition < 0 && _carDrivingType != ECarDrivingType.FrontWheelDrive)
                    wheelCollider.motorTorque = torque;

                if (wheelLocalZPosition >= 0 && _carDrivingType != ECarDrivingType.RearWheelDrive)
                    wheelCollider.motorTorque = torque;

                wheelCollider.GetWorldPose(out var p, out var q);

                wheelShape.rotation = q;
                wheelShape.position = p;
            }
        }

        private void FixedUpdate()
        {
            foreach (var wheelCollider in _wheels.Select(wheel => wheel.Collider))
            {
                if (!wheelCollider.GetGroundHit(out var hit))
                    continue;

                var fFriction = wheelCollider.forwardFriction;
                var material = hit.collider.material;

                fFriction.stiffness = material.staticFriction;
                wheelCollider.forwardFriction = fFriction;

                var sFriction = wheelCollider.sidewaysFriction;
                sFriction.stiffness = material.staticFriction;
                wheelCollider.sidewaysFriction = sFriction;
            }
        }

        public void Dispose()
        {
            gameObject.SetActive(false);
        }
    }
}