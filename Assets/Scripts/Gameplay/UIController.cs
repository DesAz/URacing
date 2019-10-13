using UnityEngine;
using UnityEngine.SceneManagement;
using URacing.Checkpoints;
using URacing.Controls;

namespace URacing.UI
{
    public class UIController : UIElement
    {
        private const string LEVEL_SELECTION_SCENE_NAME = "LevelSelection";

        [SerializeField] private Speedometer _speedometer = default;
        [SerializeField] private RoundTimer _roundTimer = default;

        [SerializeField] private PauseScreen _pauseScreen = default;
        [SerializeField] private GameFinishedPanel _gameFinishedPanel = default;
        [SerializeField] private CheckpointPassedTime _checkpointPassedTime = default;

        private float _timer;

        public void Init(PlayerCarController playerCarController)
        {
            _speedometer.Init(playerCarController);
        }

        public void UpdateTimer(float timer)
        {
            _timer = timer;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_pauseScreen.gameObject.activeSelf)
                {
                    OnResumeButtonPressed();
                }
                else
                {
                    Time.timeScale = 0.0f;
                    _pauseScreen.Init(OnResumeButtonPressed, OnRestartButtonPressed, OnMenuButtonPressed);
                }
            }

            _roundTimer.UpdateTimer(timer);
        }

        private void OnResumeButtonPressed()
        {
            Time.timeScale = 1.0f;
            _pauseScreen.Dispose();
        }

        private void OnRestartButtonPressed()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(gameObject.scene.buildIndex);
        }

        private void OnMenuButtonPressed()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(LEVEL_SELECTION_SCENE_NAME);
        }

        public void OnCheckpointPassed(Checkpoint checkpoint)
        {
            _checkpointPassedTime.CheckpointPassed(checkpoint.DesiredTime, _timer);
        }

        public void OnCheckpointGameFinished(int starsCount)
        {
            _gameFinishedPanel.Init(starsCount, () =>
            {
                _gameFinishedPanel.Dispose();

                SceneManager.LoadScene("LevelSelection");
            });
        }
    }
}