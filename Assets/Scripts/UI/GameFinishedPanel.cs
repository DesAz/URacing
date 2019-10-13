using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace URacing.UI
{
    public class GameFinishedPanel : UIElement
    {
        private const float STARS_DISPLAY_DELAY = 0.1f;

        [SerializeField] private Button _okButton = default;
        [SerializeField] private GameObject[] _starObjects = default;

        public void Init(int starsCount, Action onOkButtonPressed)
        {
            _okButton.onClick.AddListener(() => onOkButtonPressed?.Invoke());
            AddDisposable(() => _okButton.onClick.RemoveAllListeners());

            ShowGameObject();

            StartCoroutine(Co_DisplayStarsCount(starsCount));
        }

        private IEnumerator Co_DisplayStarsCount(int starsCount)
        {
            foreach (var starObject in _starObjects)
                starObject.gameObject.SetActive(false);

            for (var i = 0; i < _starObjects.Length; i++)
            {
                var star = _starObjects[i];

                if (starsCount > i)
                    star.gameObject.SetActive(true);

                yield return new WaitForSeconds(STARS_DISPLAY_DELAY);
            }
        }
    }
}