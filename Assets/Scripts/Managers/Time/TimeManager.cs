using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimeClock
{
    public class TimeManager : MonoBehaviour
    {
        public RectTransform  hourHand;
        public RectTransform  minuteHand;
        public RectTransform  secondHand;
        public TextMeshProUGUI timeText;
        public Button EditButton;

        private DateTime currentTime;
        private int lastUpdateHour = -1;
        private bool _isEdit = false;
        
        private ITimeService timeService;
        public TimeEditManager timeEditManager;

        void Start()
        {
            timeService = new YandexTimeService();
            GetTimeFromServerAsync().Forget();
            EditButton.onClick.AddListener(timeEditManager.EnterEditMode);
        }

        private async UniTaskVoid GetTimeFromServerAsync()
        {
            currentTime = await timeService.GetTimeFromServerAsync();
            if (currentTime == DateTime.MinValue)
            {
                Debug.LogError("Не удалось получить время с сервера.");
                return;
            }
        }

        void Update()
        {
            currentTime = currentTime.AddSeconds(Time.deltaTime);
            UpdateClock();
        }

        void UpdateClock()
        {
            if (currentTime == null) return;

            hourHand.localRotation = Quaternion.Euler(0, 0, -((currentTime.Hour % 12) + currentTime.Minute / 60f) * 30f);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -(currentTime.Minute + currentTime.Second / 60f) * 6f);
            secondHand.localRotation = Quaternion.Euler(0, 0, -(currentTime.Second * 6f));
            
            timeText.text = currentTime.ToString("HH:mm:ss");
            if (currentTime.Hour != lastUpdateHour && !_isEdit)
            {
                lastUpdateHour = currentTime.Hour;
                GetTimeFromServerAsync().Forget();
            }
        }

        public void SetTime(DateTime newTime)
        {
            _isEdit = true;
            currentTime = newTime;
        }

        public DateTime GetTime()
        {
            return currentTime;
        }
    }
}
