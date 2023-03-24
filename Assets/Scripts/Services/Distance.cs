using UnityEngine;
using System;
using Assets.Scripts.General;

namespace Services
{
    public class Distance : Singleton<Distance>
    {
        public Action<double> OnChanged;

        private double _value;

        private void OnEnable() => Updater.Instance.OnFixedUpdate += UpdateValue;
        
        public double Value
        {
            private set
            {
                _value = Math.Round(value, 0);

                OnChanged?.Invoke(_value);
            }

            get => _value;
        }

        public void ResetValue() => Value = 0;   

        private void UpdateValue() => Value += GlobalSpeed.Instance.Value * Time.fixedDeltaTime;

        private void OnDisable() => Updater.Instance.OnFixedUpdate -= UpdateValue;
        
    }
}