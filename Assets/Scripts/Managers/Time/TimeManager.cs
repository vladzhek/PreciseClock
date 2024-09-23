using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimeClock
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private RectTransform  _hourHand;
        [SerializeField] private RectTransform  _minuteHand;
        [SerializeField] private RectTransform  _secondHand;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private Button _editButton;
        [SerializeField] private TimeEditManager _timeEditManager;

        private DateTime _currentTime;
        private int _lastUpdateHour = -1;
        private bool _isEdit = false;
        
        private ITimeService _timeService;

        private void OnEnable()
        {
            _editButton.onClick.AddListener(_timeEditManager.EnterEditMode);
        }

        private void OnDisable()
        {
            _editButton.onClick.RemoveListener(_timeEditManager.EnterEditMode);
        }

        void Start()
        {
            _timeService = new YandexTimeService();
            GetTimeFromServerAsync().Forget();
        }

        private async UniTaskVoid GetTimeFromServerAsync()
        {
            _currentTime = await _timeService.GetTimeFromServerAsync();
            if (_currentTime == DateTime.MinValue)
            {
                Debug.LogError("Не удалось получить время с сервера.");
                return;
            }
        }

        void Update()
        {
            _currentTime = _currentTime.AddSeconds(Time.deltaTime);
            UpdateClock();
        }

        void UpdateClock()
        {
            if (_currentTime == null) return;

            _hourHand.localRotation = Quaternion.Euler(0, 0, -((_currentTime.Hour % 12) + _currentTime.Minute / 60f) * 30f);
            _minuteHand.localRotation = Quaternion.Euler(0, 0, -(_currentTime.Minute + _currentTime.Second / 60f) * 6f);
            _secondHand.localRotation = Quaternion.Euler(0, 0, -(_currentTime.Second * 6f));
            
            _timeText.text = _currentTime.ToString("HH:mm:ss");
            if (_currentTime.Hour != _lastUpdateHour && !_isEdit)
            {
                _lastUpdateHour = _currentTime.Hour;
                GetTimeFromServerAsync().Forget();
            }
        }

        public void SetTime(DateTime newTime)
        {
            _isEdit = true;
            _currentTime = newTime;
        }

        public DateTime GetTime()
        {
            return _currentTime;
        }
    }
}
