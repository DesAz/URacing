using UnityEngine;
using System;

namespace URacing.Controls
{
    [Serializable]
    public enum ECarDrivingType
    {
        RearWheelDrive,
        FrontWheelDrive,
        AllWheelDrive
    }

    public interface ICarController
    {
        string Name { get; }
    }

    public class PlayerCarController : MonoBehaviour, ICarController
    {
        [Serializable]
        private class Wheel
        {
            public WheelCollider Collider = default;
            public Transform Shape = default;
            public bool IsFront = default;

            public float LocalZPosition => Collider.transform.localPosition.z;
        }

        private const KeyCode HAND_BRAKE = KeyCode.Space;

        [SerializeField] private ECarDrivingType _carDrivingType = default;

        [SerializeField] private float _maxSteeringAngle = 30f;
        [SerializeField] private float _maxTorque = 300f;
        [SerializeField] private float _maxBrakeTorque = 30000f;

        [SerializeField] private float _criticalEngineSpeed = 5f;
        [SerializeField] private int _stepsBelowCritical = 5;
        [SerializeField] private int _stepsAboveCritical = 1;

        [SerializeField] private Wheel[] _wheels = default;

        public string Name => "PlayerController";

        private void Update()
        {
            foreach (var wheel in _wheels)
                wheel.Collider.ConfigureVehicleSubsteps(_criticalEngineSpeed, _stepsBelowCritical, _stepsAboveCritical);

            var angle = _maxSteeringAngle * Input.GetAxis("Horizontal");
            var torque = _maxTorque * Input.GetAxis("Vertical");

            var handBrake = Input.GetKey(HAND_BRAKE) ? _maxBrakeTorque : 0;

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

                if (wheel.IsFront)
                {
                    wheelShape.rotation = q * Quaternion.Euler(0, 180, 0);
                    wheelShape.position = p;
                }
                else
                {
                    wheelShape.position = p;
                    wheelShape.rotation = q;
                }
            }
        }
    }
}