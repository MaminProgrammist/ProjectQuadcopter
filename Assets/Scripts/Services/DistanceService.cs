using UnityEngine;
using System;
using Assets.Scripts.General;

namespace Services
{
    public class DistanceService : Singleton<DistanceService>
    {
        public Action<double> OnChanged;

        private double _distance;

        private void OnEnable() => UpdateService.Instance.OnFixedUpdate += UpdateDistance;
        
        public double Distance
        {
            private set
            {
                _distance = Math.Round(value, 0);

                OnChanged?.Invoke(_distance);
            }

            get => _distance;
        }

        public void ResetDistance() => Distance = 0;   

        private void UpdateDistance() => Distance += GlobalSpeedService.Speed * Time.fixedDeltaTime;

        private void OnDisable() => UpdateService.Instance.OnFixedUpdate -= UpdateDistance;
        
    }
}