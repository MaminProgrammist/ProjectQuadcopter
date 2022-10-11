using System;
using Other;
using UnityEngine;

namespace Services
{
    public class ScoreService : MonoBehaviour
    {
        public static event Action OnDistanceNewRecord;
        public static event Action OnMoneyNewRecord;

        private void OnEnable()
        {
            OnMoneyNewRecord += NewRecord;
            OnDistanceNewRecord += NewRecord;
        }

        private void NewRecord()
        {
            Debug.Log("POSOSI");
        }

        public static void CheckRecord()
        {
            SerializedData currentData = DataService.Data;
            SerializedData updatedData = currentData;

            if (MoneyService.Money > currentData.MoneyRecord)
            {
                updatedData.MoneyRecord = MoneyService.Money;

                OnMoneyNewRecord?.Invoke();
            }

            if (DistanceService.Distance > currentData.DistanceRecord)
            {
                updatedData.DistanceRecord = DistanceService.Distance;

                OnDistanceNewRecord?.Invoke();
            }

            DataService.UpdateData(updatedData);
        }

        private void OnDisable()
        {
            OnMoneyNewRecord -= NewRecord;
            OnDistanceNewRecord -= NewRecord;
        }
    }
}