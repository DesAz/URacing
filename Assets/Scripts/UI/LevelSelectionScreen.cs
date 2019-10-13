using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace URacing.UI
{
    public class LevelSelectionScreen : UIElement
    {
        [SerializeField] private SelectionToggle _levelsSelectionToggle = default;
        [SerializeField] private SelectionToggle _vehiclesSelectionToggle = default;

        [SerializeField] private GameObject[] _starsObjects = default;
        [SerializeField] private TextMeshProUGUI _bestLevelTime = default;

        [SerializeField] private Button _startLevelButton = default;

        public static string CachedLevelKey;

        private readonly Dictionary<string, SavedScore> _cachedStarsCount = new Dictionary<string, SavedScore>();

        private int _currentLevelIndex;
        private string _selectedLevel;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _startLevelButton.onClick.AddListener(StartLevel);
            AddDisposable(_startLevelButton.onClick.RemoveAllListeners);

            _levelsSelectionToggle.Init(CachedLevelKey, CurrentLevelToggleChanged);
            _vehiclesSelectionToggle.Init(AvailableVehicles.CachedVehicleKey, CurrentVehicleToggleChanged);
        }

        private void CurrentLevelToggleChanged()
        {
            _selectedLevel = _levelsSelectionToggle.SelectedValue;

            CachedLevelKey = _selectedLevel;

            var sceneKey = $"{_selectedLevel} Score";
            SavedScore savedScore;

            if (_cachedStarsCount.ContainsKey(sceneKey))
            {
                savedScore = _cachedStarsCount[sceneKey];
            }
            else
            {
                savedScore = JsonUtility.FromJson<SavedScore>(PlayerPrefs.GetString(sceneKey));
                _cachedStarsCount.Add(sceneKey, savedScore);
            }

            if (savedScore == null)
                savedScore = new SavedScore();

            for (var i = 0; i < _starsObjects.Length; i++)
            {
                var star = _starsObjects[i];
                star.SetActive(savedScore.StarsCount > i);
            }

            var savedTime = Mathf.RoundToInt(savedScore.Time);

            if (savedTime < 0)
                savedTime = 0;

            _bestLevelTime.text = savedTime + " seconds best";
        }

        private void CurrentVehicleToggleChanged()
        {
            AvailableVehicles.CachedVehicleKey = _vehiclesSelectionToggle.SelectedValue;
        }

        private void StartLevel()
        {
            if (string.IsNullOrEmpty(_selectedLevel))
                throw new Exception("Selected level is null somehow");

            SceneManager.LoadScene(_selectedLevel);
        }
    }
}