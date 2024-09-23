using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TimeClock
{
    public class TimeApiService : ITimeService
    {
        private const string _url = "https://timeapi.io/api/Time/current/zone?timeZone=";
        private string timeZone;

        public TimeApiService()//string timeZone)
        {
            //TODO: можно раcширить для разных поясов
            this.timeZone = "Europe/Moscow"; //timeZone;
        }

        public async UniTask<DateTime> GetTimeFromServerAsync()
        {
            var requestUrl = $"{_url}{timeZone}";

            var request = UnityWebRequest.Get(requestUrl);
            var response = await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Ошибка получения времени с TimeAPI: " + request.error);
                return DateTime.MinValue;
            }
            else
            {
                var timeResponse = JsonUtility.FromJson<TimeApiResponse>(response.downloadHandler.text);
                if (timeResponse != null)
                {
                    return DateTime.Parse(timeResponse.dateTime);
                }
                else
                {
                    Debug.LogError("Ошибка в данных ответа TimeAPI.");
                    return DateTime.MinValue;
                }
            }
        }
    }
}