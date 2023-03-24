using Assets.Scripts.General;
using System;
using UnityEngine;

namespace Services
{
    public class GlobalSpeed : Singleton<GlobalSpeed>
    {
        public event Action OnStartup;
        public event Action OnStop;

        [SerializeField, Range(0, 100)] private float _initial;

        public float Value { get; private set; }
        public float Acceleration => 0.1f;

        private void OnEnable()
        {
            Updater.Instance.OnUpdate += SpeedUp;
            Value = _initial;
            OnStartup?.Invoke();
        }

        private void SpeedUp() => Value += Acceleration * Time.deltaTime;

        private void OnDisable()
        {
            Updater.Instance.OnUpdate -= SpeedUp;
            Value = 0;
            OnStop?.Invoke();
        }
    }
}