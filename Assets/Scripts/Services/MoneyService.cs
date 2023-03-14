using UnityEngine;
using System;
using Assets.Scripts.General;

namespace Services
{
    public class MoneyService : Singleton<MoneyService>
    {
        public event Action<int> OnChanged;

        private int _money;

        public int Money
        {
            get => _money;

            private set
            {
                _money = Mathf.Clamp(value, 0, int.MaxValue);

                OnChanged?.Invoke(_money);
            }
        }

        public void AddMoney(int money) => Money += money;

        public void SubtractMoney(int money) => Money -= money;

        public void SetInitialAmount() => Money = 0;
    }
}
