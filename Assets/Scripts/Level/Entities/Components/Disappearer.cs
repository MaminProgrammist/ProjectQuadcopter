using System;
using UnityEngine;
using General;
using Services;

namespace Components
{
    public class Disappearer : MonoBehaviour
    {
        public event Action OnDisappear;

        private WayMatrix _wayMatrix = new();

        private void OnEnable() => Updater.Instance.OnUpdate += CheckEdgeOut;

        private void CheckEdgeOut()
        {
            if (transform.position.z <= _wayMatrix.DisappearPoint)
            {
                OnDisappear?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void OnDisable() => Updater.Instance.OnUpdate -= CheckEdgeOut;
    }
}