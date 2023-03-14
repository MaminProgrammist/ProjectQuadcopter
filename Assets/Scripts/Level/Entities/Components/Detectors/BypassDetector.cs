using System;
using UnityEngine;
using Services;
using Entities;

namespace Components
{
    [Obsolete]
    public class BypassDetector : Detector
    {
        public override event Action<Entity> OnDetect;
        public override event Action OnDetectAll;

        private bool _isDetection = true;
        private bool _bypassCondition = true;
        private Entity _target;

        private void OnEnable()
        {
            UpdateService.Instance.OnUpdate += Detect;
            _target = FindObjectOfType<Quadcopter>();
            _bypassCondition = true;
        }

        public void Disactivate() =>_bypassCondition = false;

        private void Detect()
        {
            if (CheckBypass() && _isDetection)
            {
                if (_bypassCondition)
                {
                    OnDetectAll?.Invoke();
                    OnDetect?.Invoke(_target);
                }
                _bypassCondition = true;
                _isDetection = false;
            }

            if (CheckBypass() == false && _isDetection == false)
                _isDetection = true;
        }

        private bool CheckBypass() { return transform.position.z + _config.XDetectionDistanceLeft < _target.transform.position.z; }
        
        private void OnDisable() => UpdateService.Instance.OnUpdate -= Detect;
    }
}