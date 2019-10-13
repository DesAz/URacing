using UnityEngine;

namespace URacing
{
    public class DirectionArrow : MonoBehaviour
    {
        [SerializeField] private Transform _arrowTransform = default;

        private ICameraTarget _followCheckpoint;

        public void Init(ICameraTarget startingPoint)
        {
            var arrowTransform = transform;

            arrowTransform.SetParent(startingPoint.Follow);

            arrowTransform.localPosition = new Vector3(0, 4, 5);
            arrowTransform.rotation = Quaternion.identity;
        }

        public void SetFollowTarget(ICameraTarget checkpoint)
        {
            _followCheckpoint = checkpoint;
        }

        private void Update()
        {
            if (_followCheckpoint == null)
                return;

            _arrowTransform.LookAt(_followCheckpoint.Follow, _arrowTransform.up);

            var x = _arrowTransform.rotation;

            x.x = 0;
            x.z = 0;

            _arrowTransform.rotation = x;
        }
    }
}