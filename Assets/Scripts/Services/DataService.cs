using UnityEngine;
using System.IO;
using Other;
using System;
using Assets.Scripts.General;

namespace Services
{
    public class DataService : Singleton<DataService>
    {
        private string _dataFileName = "data.json";
        private string _dataFilePath;
        private SerializedData _data;

        public SerializedData Data 
        {
            get => _data;

            private set
            {
                _data = value;
            }
        }

        protected override void Init()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                _dataFilePath = Path.Combine(Application.persistentDataPath, _dataFileName);
            #else
            _dataFilePath = Path.Combine(Application.persistentDataPath, _dataFileName);
            #endif

            LoadDataFromFile();
        }

        public void LoadDataFromFile()
        {
            if (File.Exists(_dataFilePath))
            {
                try
                {
                    string dataAsJson = File.ReadAllText(_dataFilePath);
                    Data = JsonUtility.FromJson<SerializedData>(dataAsJson);
                }
                catch (Exception e)
                {
                    Debug.Log("A problem detected while working with file! " + e.Message);
                }
            }
            else
            {
                Data = new SerializedData(0, 0);
            }
        }

        public void UpdateData(SerializedData newData)
        {
            Data = newData;
            SaveDataToFile();
        }

        public void SaveDataToFile()
        {
            string dataAsJson = JsonUtility.ToJson(Data, true);

            try
            {
                File.WriteAllText(_dataFilePath, dataAsJson);
            }
            catch (Exception e)
            {
                Debug.Log("A problem detected while working with file! " + e.Message);
            }
        }
    }
}