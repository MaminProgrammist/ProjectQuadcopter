using UnityEngine;
using System;

namespace Services
{
    public class DistanceService : MonoBehaviour
    {
        public static Action<double> OnChanged;

        private double _distance;

        private void OnEnable() => UpdateService.OnFixedUpdate += UpdateDistance;
        
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

        private void OnDisable() => UpdateService.OnFixedUpdate -= UpdateDistance;
        
    }
}