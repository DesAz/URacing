using System;
using System.Collections.Generic;
using UnityEngine;

namespace URacing
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = default;

        [SerializeField] private float _minZoomDistance = 10f;
        [SerializeField] private float _maxZoomDistance = 50f;

        [SerializeField] private float _smoothing = 0.5f;

        [SerializeField] private float _minY = 10f;
        [SerializeField] private float _maxY = 40f;

        private readonly List<ICameraTarget> _targets = new List<ICameraTarget>();
        private Vector3 _velocity;

        public void AddTarget(ICameraTarget target)
        {
            if (_targets.Contains(target))
                throw new Exception("Targets list already contain: " + target.Follow.name);

            _targets.Add(target);
        }

        public void RemoveTarget(ICameraTarget target)
        {
            if (!_targets.Contains(target))
                throw new Exception("Targets list does not contain: " + target.Follow.name);

            _targets.Remove(target);
        }

        public void ClearTargets()
        {
            for (var i = _targets.Count - 1; i >= 0; i--)
            {
                var target = _targets[i];

                if (target.ForceShow)
                    continue;

                RemoveTarget(target);
            }
        }

        private void LateUpdate()
        {
            if (_targets.Count <= 0)
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
            if (_targets.Count == 1)
                return _targets[0].Follow.position;

            var center = bounds.center;
            center.y = 0f;

            return center;
        }

        private Bounds EncapsulateTargets()
        {
            var bounds = new Bounds(_targets[0].Follow.position, Vector3.zero);

            foreach (var target in _targets)
                bounds.Encapsulate(target.Follow.position);

            return bounds;
        }
    }
}