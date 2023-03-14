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

        private void OnEnable() => UpdateService.Instance.OnUpdate += CheckEdgeOut;

        private void CheckEdgeOut()
        {
            if (transform.position.z <= _wayMatrix.DisappearPoint)
            {
                OnDisappear?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void OnDisable() => UpdateService.Instance.OnUpdate -= CheckEdgeOut;
    }
}