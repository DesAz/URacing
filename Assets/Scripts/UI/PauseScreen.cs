using System;
using UnityEngine;
using UnityEngine.UI;

namespace URacing.UI
{
    public class PauseScreen : UIElement
    {
        [SerializeField] private Button _resumeButton = default;
        [SerializeField] private Button _restartButton = default;
        [SerializeField] private Button _menuButton = default;

        private Action _onResumeButtonPressed;
        private Action _onRestartButtonPressed;
        private Action _onMenuButtonPressed;

        public void Init(Action onResumeButtonPressed, Action onRestartButtonPressed, Action onMenuButtonPressed)
        {
            ShowGameObject();

            _onResumeButtonPressed = onResumeButtonPressed;
            _onRestartButtonPressed = onRestartButtonPressed;
            _onMenuButtonPressed = onMenuButtonPressed;

            _resumeButton.onClick.AddListener(() => _onResumeButtonPressed?.Invoke());
            _restartButton.onClick.AddListener(() => _onRestartButtonPressed?.Invoke());
            _menuButton.onClick.AddListener(() => _onMenuButtonPressed?.Invoke());

            AddDisposable(_resumeButton.onClick.RemoveAllListeners);
            AddDisposable(_restartButton.onClick.RemoveAllListeners);
            AddDisposable(_menuButton.onClick.RemoveAllListeners);
        }
    }
}