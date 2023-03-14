using System;
using Assets.Scripts.General;
using Other;
using UnityEngine;

namespace Services
{
    public class ScoreService : Singleton<ScoreService>
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
            SerializedData currentData = DataService.Instance.Data;
            SerializedData updatedData = currentData;

            if (MoneyService.Instance.Money > currentData.MoneyRecord)
            {
                updatedData.MoneyRecord = MoneyService.Instance.Money;

                OnMoneyNewRecord?.Invoke();
            }

            if (DistanceService.Instance.Distance > currentData.DistanceRecord)
            {
                updatedData.DistanceRecord = DistanceService.Instance.Distance;

                OnDistanceNewRecord?.Invoke();
            }

            DataService.Instance.UpdateData(updatedData);
        }

        private void OnDisable()
        {
            OnMoneyNewRecord -= NewRecord;
            OnDistanceNewRecord -= NewRecord;
        }
    }
}