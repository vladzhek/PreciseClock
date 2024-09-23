using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimeClock
{
    public class TimeEditManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _hourInputField;
        [SerializeField] private TMP_InputField _minuteInputField;
        [SerializeField] private TMP_InputField _secondInputField;
        [SerializeField] private Button saveButton;
        [SerializeField] private TimeManager timeManager;

        private DateTime currentTime;

        private void OnEnable()
        {
            saveButton.onClick.AddListener(SaveTimeFromInput);
        }

        private void OnDisable()
        {
            saveButton.onClick.RemoveListener(SaveTimeFromInput);
        }

        private void SetActiveEdit(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void EnterEditMode()
        {
            SetActiveEdit(true);
            
            currentTime = timeManager.GetTime();
            UpdateInputs(currentTime);
        }
        
        private void UpdateInputs(DateTime time)
        {
            _hourInputField.text = time.Hour.ToString();
            _minuteInputField.text = time.Minute.ToString();
            _secondInputField.text = time.Second.ToString();
        }

        public void ExitEditMode()
        {
            gameObject.SetActive(false);
            timeManager.SetTime(currentTime);
        }

        private void SaveTimeFromInput()
        {
            if (int.TryParse(_hourInputField.text, out int newHour) &&
                int.TryParse(_minuteInputField.text, out int newMinute) &&
                int.TryParse(_secondInputField.text, out int newSecond))
            {
                if (newHour >= 0 && newHour < 24 && newMinute >= 0 && newMinute < 60 && newSecond >= 0 && newSecond < 60)
                {
                    currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, newHour, newMinute,
                        newSecond);

                    ExitEditMode();
                }
                else
                {
                    Debug.LogError("Некорректное время!");
                }
            }
            else
            {
                Debug.LogError("Некорректный ввод!");
            }
        }
    }
}