using System;
using System.Collections.Generic;
using UnityEngine;

namespace URacing.UI
{
    public class UIElement : MonoBehaviour, IDisposable
    {
        private readonly List<Action> _destroys = new List<Action>();

        public void AddDisposable(Action destroy)
        {
            _destroys.Add(destroy);
        }

        public void Dispose()
        {
            foreach (var destroy in _destroys)
                destroy();

            _destroys.Clear();

            HideGameObject();
        }

        protected void ShowGameObject()
        {
            gameObject.SetActive(true);
        }

        private void HideGameObject()
        {
            gameObject.SetActive(false);
        }
    }
}