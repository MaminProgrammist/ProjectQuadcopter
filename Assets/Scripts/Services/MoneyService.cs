using UnityEngine;
using System;

namespace Services
{
    public class MoneyService : MonoBehaviour
    {
        public static event Action<int> OnChanged;

        private static int _money;

        public static int Money
        {
            get => _money;

            private set
            {
                _money = Mathf.Clamp(value, 0, int.MaxValue);

                OnChanged?.Invoke(_money);
            }
        }

        public static void AddMoney(int money) => Money += money;

        public static void SubtractMoney(int money) => Money -= money;

        public static void SetInitialAmount() => Money = 0;
    }
}
