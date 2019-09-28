using UnityEngine;

namespace URacing
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform[] _targets = default;
        [SerializeField] private Vector3 _offset = default;

        [SerializeField] private float _minZoomDistance = 10f;
        [SerializeField] private float _maxZoomDistance = 50f;

        [SerializeField] private float _smoothing = 0.5f;

        [SerializeField] private float _minY = 10f;
        [SerializeField] private float _maxY = 40f;

        private Vector3 _velocity;

        public void Init()
        {
        }

        private void LateUpdate()
        {
            if (_targets.Length <= 0)
                return;

            var bounds = EncapsulateTargets();

            Move(CenterPoint(bounds));
            Zoom(bounds);
        }

        private void Move(Vector3 centerPoint)
        {
            var cameraTransform = transform;
            var position = cameraTransform.position;

            centerPoint.y = position.y;
            centerPoint += _offset;

            position = Vector3.SmoothDamp(position, centerPoint, ref _velocity, _smoothing);
            cameraTransform.position = position;
        }

        private void Zoom(Bounds bounds)
        {
            var xBoundsSize = bounds.size.x;
            var zBoundsSize = bounds.size.z;

            var greatestDistance = xBoundsSize > zBoundsSize ? xBoundsSize : zBoundsSize;

            if (greatestDistance < _minZoomDistance)
                greatestDistance = 0;

            var newY = Mathf.Lerp(_minY, _maxY, greatestDistance / _maxZoomDistance);
            var position = transform.position;

            position = new Vector3(position.x,
                Mathf.Lerp(position.y, newY, Time.deltaTime), position.z);

            transform.position = position;
        }

        private Vector3 CenterPoint(Bounds bounds)
        {
            if (_targets.Length == 1)
                return _targets[0].position;

            var center = bounds.center;
            center.y = 0f;

            return center;
        }

        private Bounds EncapsulateTargets()
        {
            var bounds = new Bounds(_targets[0].position, Vector3.zero);

            foreach (var target in _targets)
                bounds.Encapsulate(target.position);

            return bounds;
        }
    }
}