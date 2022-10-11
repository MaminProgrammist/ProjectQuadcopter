using UnityEngine;
using System;

namespace Services
{
    public class DistanceService : MonoBehaviour
    {
        public static Action<double> OnChanged;

        private static double _distance;

        private void OnEnable() => UpdateService.OnFixedUpdate += UpdateDistance;
        
        public static double Distance
        {
            private set
            {
                _distance = Math.Round(value, 0);

                OnChanged?.Invoke(_distance);
            }

            get => _distance;
        }

        public static void ResetDistance() => Distance = 0;   

        private void UpdateDistance() => Distance += GlobalSpeedService.Speed * Time.fixedDeltaTime;

        private void OnDisable() => UpdateService.OnFixedUpdate -= UpdateDistance;
        
    }
}