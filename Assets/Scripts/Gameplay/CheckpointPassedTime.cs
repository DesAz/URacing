using System.Collections;
using TMPro;
using UnityEngine;

namespace URacing.UI
{
    public class CheckpointPassedTime : UIElement
    {
        private const float FADE_TIME = 3f;

        [SerializeField] private TextMeshProUGUI _checkpointTimeLabel = default;
        [SerializeField] private TextMeshProUGUI _currentTimeLabel = default;

        private Coroutine _showTimeCoroutine;

        public void CheckpointPassed(float desiredTime, float currentTime)
        {
            if (_showTimeCoroutine != null)
                StopCoroutine(_showTimeCoroutine);

            _showTimeCoroutine = StartCoroutine(Co_ShowTime(desiredTime, currentTime));
        }

        private IEnumerator Co_ShowTime(float checkpointTime, float currentTime)
        {
            _checkpointTimeLabel.text = $"Desired time: {Mathf.RoundToInt(checkpointTime).ToString()}";
            _currentTimeLabel.text = $"Current time: {Mathf.RoundToInt(currentTime).ToString()}";

            yield return new WaitForSeconds(FADE_TIME);

            _checkpointTimeLabel.text = string.Empty;
            _currentTimeLabel.text = string.Empty;
        }
    }
}