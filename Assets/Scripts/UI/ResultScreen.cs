using UnityEngine;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;

    private void Start()
    {
        GameTimeManager.Instance.OnVictory += ShowVictory;
        GameTimeManager.Instance.OnDefeat += ShowDefeat;
    }

    private void ShowVictory()
    {
        resultPanel.SetActive(true);
        resultText.text = "VICTORIA";
    }

    private void ShowDefeat()
    {
        resultPanel.SetActive(true);
        resultText.text = "DERROTA";
    }

    private void OnDisable()
    {
        if (GameTimeManager.Instance != null)
        {
            GameTimeManager.Instance.OnVictory -= ShowVictory;
            GameTimeManager.Instance.OnDefeat -= ShowDefeat;
        }
    }
}