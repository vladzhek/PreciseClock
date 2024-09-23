using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimeClock
{
    public class TimeEditManager : MonoBehaviour
    {
        public TMP_InputField hourInputField;
        public TMP_InputField minuteInputField;
        public TMP_InputField secondInputField;
        public Button saveButton;
        
        public TimeManager timeManager;

        private DateTime currentTime;

        public void Start()
        {
            gameObject.SetActive(false);
            saveButton.onClick.AddListener(SaveTimeFromInput);
        }

        private void UpdateInputs(DateTime time)
        {
            hourInputField.text = time.Hour.ToString();
            minuteInputField.text = time.Minute.ToString();
            secondInputField.text = time.Second.ToString();
        }

        public void EnterEditMode()
        {
            gameObject.SetActive(true);
            
            currentTime = timeManager.GetTime();
            UpdateInputs(currentTime);
        }

        public void ExitEditMode()
        {
            gameObject.SetActive(false);
            timeManager.SetTime(currentTime);
        }

        public void SaveTimeFromInput()
        {
            if (int.TryParse(hourInputField.text, out int newHour) &&
                int.TryParse(minuteInputField.text, out int newMinute) &&
                int.TryParse(secondInputField.text, out int newSecond))
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