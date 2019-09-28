namespace URacing.Controls
{
    using UnityEngine;

    [ExecuteInEditMode]
    public class CarSuspension : MonoBehaviour
    {
        [Range(0.1f, 20f)]
        [SerializeField]
        private float _naturalFrequency = 10;

        [Range(0f, 3f)]
        [SerializeField]
        private float _dampingRatio = 0.8f;

        [Range(-1f, 1f)]
        [SerializeField]
        private float _forceShift = 0.03f;

        [SerializeField] private bool _setSuspensionDistance = true;
        [SerializeField] private Rigidbody m_Rigidbody = default;

        [SerializeField] private WheelCollider[] _wheelColliders = default;

        private void Update()
        {
            foreach (var wheelCollider in _wheelColliders)
            {
                var spring = wheelCollider.suspensionSpring;

                var sqrtWcSprungMass = Mathf.Sqrt(wheelCollider.sprungMass);

                spring.spring = sqrtWcSprungMass * _naturalFrequency * sqrtWcSprungMass * _naturalFrequency;
                spring.damper = 2f * _dampingRatio * Mathf.Sqrt(spring.spring * wheelCollider.sprungMass);

                wheelCollider.suspensionSpring = spring;

                var wheelRelativeBody = transform.InverseTransformPoint(wheelCollider.transform.position);
                var distance = m_Rigidbody.centerOfMass.y - wheelRelativeBody.y + wheelCollider.radius;

                wheelCollider.forceAppPointDistance = distance - _forceShift;

                if (spring.targetPosition > 0 && _setSuspensionDistance)
                {
                    wheelCollider.suspensionDistance = wheelCollider.sprungMass * Physics.gravity.magnitude /
                                                       (spring.targetPosition * spring.spring);
                }
            }
        }
    }
}