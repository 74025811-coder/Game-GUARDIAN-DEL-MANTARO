using UnityEngine;
using TMPro;

public class TimerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private void Start()
    {
        GameTimeManager.Instance.OnTimeChanged += UpdateText;
    }

    private void UpdateText(float secondsRemaining)
    {
        int minutes = Mathf.FloorToInt(secondsRemaining / 60f);
        int seconds = Mathf.FloorToInt(secondsRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnDisable()
    {
        if (GameTimeManager.Instance != null)
        {
            GameTimeManager.Instance.OnTimeChanged -= UpdateText;
        }
    }
}