using TMPro;
using UnityEngine;
using UnityEngine.UI;
using URacing.Controls;

namespace URacing.UI
{
    public class Speedometer : UIElement
    {
        private const float MPH_CONVERSION = 2.23694f;

        [SerializeField] private TextMeshProUGUI _speedometerValue = default;
        [SerializeField] private Image _speedometerImage = default;

        private PlayerCarController _playerCarController;

        public void Init(PlayerCarController playerCarController)
        {
            _playerCarController = playerCarController;
        }

        private void Update()
        {
            if (_playerCarController == null)
                return;

            var currentSpeed = _playerCarController.Rigidbody.velocity.magnitude * MPH_CONVERSION;
            _speedometerValue.text = $"{Mathf.RoundToInt(currentSpeed)} MPh";

            _speedometerImage.fillAmount = currentSpeed / 100f;
        }
    }
}