using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace URacing.UI
{
    [Serializable]
    public class Selectable
    {
        public Sprite Sprite = default;
        public string Value = default;
    }

    public class SelectionToggle : UIElement
    {
        [SerializeField] private Selectable[] _selectables = default;

        [SerializeField] private Image _currentImage = default;
        [SerializeField] private TextMeshProUGUI _currentName = default;

        [SerializeField] private Button _leftArrowButton = default;
        [SerializeField] private Button _rightArrowButton = default;

        private Action _onToggleChanged;

        private int _currentToggleIndex;
        public string SelectedValue => _selectables[_currentToggleIndex].Value;

        public void Init(string savedKey, Action onToggleChanged)
        {
            _onToggleChanged = onToggleChanged;

            _leftArrowButton.onClick.AddListener(() =>
            {
                _currentToggleIndex--;
                CurrentLevelToggleChanged();
            });

            _rightArrowButton.onClick.AddListener(() =>
            {
                _currentToggleIndex++;
                CurrentLevelToggleChanged();
            });

            var index = -1;

            for (var i = 0; i < _selectables.Length; i++)
            {
                var selectable = _selectables[i];

                if (selectable.Value == savedKey)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
                _currentToggleIndex = index;

            CurrentLevelToggleChanged();
        }

        private void CurrentLevelToggleChanged()
        {
            var minLevel = 0;
            var maxLevel = _selectables.Length - 1;

            _currentToggleIndex = Mathf.Clamp(_currentToggleIndex, minLevel, maxLevel);

            _leftArrowButton.gameObject.SetActive(_currentToggleIndex > minLevel);
            _rightArrowButton.gameObject.SetActive(_currentToggleIndex < maxLevel);

            var selected = _selectables[_currentToggleIndex];

            _currentImage.sprite = selected.Sprite;
            _currentName.text = selected.Value;

            _onToggleChanged?.Invoke();
        }
    }
}