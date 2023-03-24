using UnityEngine;
using System;
using Assets.Scripts.General;

namespace Services
{
    public class Money : Singleton<Money>
    {
        public event Action<int> OnChanged;

        private int _value;

        public int Value
        {
            get => _value;

            private set
            {
                _value = Mathf.Clamp(value, 0, int.MaxValue);

                OnChanged?.Invoke(_value);
            }
        }

        public void Add(int amount) => Value += amount;

        public void Spend(int amount) => Value -= amount;

        public void SetInitialAmount() => Value = 0;
    }
}
