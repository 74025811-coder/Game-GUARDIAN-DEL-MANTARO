using UnityEngine;
using TMPro;

public class PollutionHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text pollutionText;

    private void Start()
    {
        PollutionManager.Instance.OnPollutionChanged += UpdateText;
        UpdateText(PollutionManager.Instance.PollutionPercentage); // fuerza el primer valor
    }

    private void OnDisable()
    {
        if (PollutionManager.Instance != null)
        {
            PollutionManager.Instance.OnPollutionChanged -= UpdateText;
        }
    }

    private void UpdateText(float percentage)
    {
        pollutionText.text = $"Contaminación: {percentage:F0}%";
    }
}