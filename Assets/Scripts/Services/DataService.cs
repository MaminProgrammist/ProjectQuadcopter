using UnityEngine;
using System.IO;
using Other;
using System;

namespace Services
{
    public class DataService : MonoBehaviour
    {
        private static string _dataFileName = "data.json";
        private static string _dataFilePath;
        private static SerializedData _data;

        public static SerializedData Data 
        {
            get => _data;

            private set
            {
                _data = value;
            }
        }

        private void Awake()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                _dataFilePath = Path.Combine(Application.persistentDataPath, _dataFileName);
            #else
                _dataFilePath = Path.Combine(Application.dataPath, _dataFileName);
            #endif

            LoadDataFromFile();
        }

        public static void LoadDataFromFile()
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

        public static void UpdateData(SerializedData newData)
        {
            Data = newData;
            SaveDataToFile();
        }

        public static void SaveDataToFile()
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