using Assets.Scripts.General;
using System;
using UnityEngine;

namespace Services
{
    public class GlobalSpeedService : Singleton<GlobalSpeedService>
    {
        public static event Action OnStartup;
        public static event Action OnStop;

        [SerializeField][Range(0, 100)]private float _speed;

        public static float Speed { get; private set; }
        public static float Acceleration => 0.1f;

        private void OnEnable()
        {
            UpdateService.Instance.OnUpdate += SpeedUp;
            Speed = _speed;
            OnStartup?.Invoke();
        }

        private static void SpeedUp() => Speed += Acceleration * Time.deltaTime;

        private void OnDisable()
        {
            UpdateService.Instance.OnUpdate -= SpeedUp;
            Speed = 0;
            OnStop?.Invoke();
        }
    }
}