using TMPro;
using UnityEngine;

namespace URacing.UI
{
    public class RoundTimer : UIElement
    {
        [SerializeField] private TextMeshProUGUI _timer = default;

        public void UpdateTimer(float time)
        {
            _timer.text = Mathf.RoundToInt(time).ToString();
        }
    }
}