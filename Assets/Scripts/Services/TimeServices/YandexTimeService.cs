using System;
using Cysharp.Threading.Tasks;
using TimeClock;
using UnityEngine;
using UnityEngine.Networking;

namespace TimeClock
{
    public class YandexTimeService : ITimeService
    {
        private const string _url = "https://yandex.com/time/sync.json";
        public async UniTask<DateTime> GetTimeFromServerAsync()
        {
            var request = UnityWebRequest.Get(_url);
            await request.SendWebRequest();
    
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Ошибка получения времени с Яндекс API: " + request.error);
                return DateTime.MinValue;
            }
            
            var timeResponse = JsonUtility.FromJson<TimeYandexResponse>(request.downloadHandler.text);
            var timestamp = timeResponse.time;
            
            var utcTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);

            return localTime;
        }
    }
}