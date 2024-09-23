using System;

namespace TimeClock
{
    [Serializable]
    public class TimeApiResponse
    {
        public string dateTime;
        public string timeZone;
        public int rawOffset;
        public int dstOffset;
    }
}