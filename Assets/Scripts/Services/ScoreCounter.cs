using System;
using Assets.Scripts.General;
using Other;
using UnityEngine;

namespace Services
{
    public class ScoreCounter : Singleton<ScoreCounter>
    {
        public event Action OnDistanceNewRecord;
        public event Action OnMoneyNewRecord;

        private void OnEnable()
        {
            OnMoneyNewRecord += NewRecord;
            OnDistanceNewRecord += NewRecord;
        }

        private void NewRecord()
        {
            Debug.Log("POSOSI");
        }

        public void CheckRecord()
        {
            SerializedData currentData = Storage.Instance.Data;
            SerializedData updatedData = currentData;

            if (Money.Instance.Value > currentData.MoneyRecord)
            {
                updatedData.MoneyRecord = Money.Instance.Value;

                OnMoneyNewRecord?.Invoke();
            }

            if (Distance.Instance.Value > currentData.DistanceRecord)
            {
                updatedData.DistanceRecord = Distance.Instance.Value;

                OnDistanceNewRecord?.Invoke();
            }

            Storage.Instance.UpdateData(updatedData);
        }

        private void OnDisable()
        {
            OnMoneyNewRecord -= NewRecord;
            OnDistanceNewRecord -= NewRecord;
        }
    }
}