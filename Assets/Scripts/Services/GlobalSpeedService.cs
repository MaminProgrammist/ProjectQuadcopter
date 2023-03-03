using Assets.Scripts.General;
using System;
using UnityEngine;

namespace Services
{
    public class GlobalSpeedService : Singleton<GlobalSpeedService>
    {
        public event Action OnStartup;
        public event Action OnStop;

        [SerializeField][Range(0, 100)]private float _speed;

        public float Speed { get; private set; }
        public float Acceleration => 0.1f;

        private void OnEnable()
        {
            UpdateService.Instance.OnUpdate += SpeedUp;
            Speed = _speed;
            OnStartup?.Invoke();
        }

        private void SpeedUp() => Speed += Acceleration * Time.deltaTime;

        private void OnDisable()
        {
            UpdateService.Instance.OnUpdate -= SpeedUp;
            Speed = 0;
            OnStop?.Invoke();
        }
    }
}