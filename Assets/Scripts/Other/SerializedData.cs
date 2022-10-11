using System;

namespace Other
{
    [Serializable]
    public struct SerializedData
    {
        public int MoneyRecord;
        public double DistanceRecord;

        public SerializedData(int money, int distanceRecord)
        {
            MoneyRecord = money;
            DistanceRecord = distanceRecord;
        }
    }
}
