using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WaveHUD : MonoBehaviour
{
    [SerializeField] private Image dotPrefab;
    [SerializeField] private Transform dotsContainer;
    [SerializeField] private GameObject warningRoot;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private float warningDisplayTime = 4f;

    [SerializeField] private Color pendingColor = new Color(1, 1, 1, 0.3f);
    [SerializeField] private Color currentColor = Color.yellow;
    [SerializeField] private Color doneColor = new Color(0.3f, 0.8f, 0.3f);

    private WasteSpawner spawner;
    private List<Image> dots = new List<Image>();

    private void Start()
    {
        spawner = FindFirstObjectByType<WasteSpawner>();
        if (spawner == null) return;

        for (int i = 0; i < spawner.TotalWaves; i++)
        {
            Image dot = Instantiate(dotPrefab, dotsContainer);
            dot.gameObject.SetActive(true);
            dots.Add(dot);
        }

        spawner.OnWaveIncoming += ShowWarning;
        spawner.OnWaveStarted += UpdateDots;

        warningRoot.SetActive(false);
        UpdateDots(1);
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnWaveIncoming -= ShowWarning;
            spawner.OnWaveStarted -= UpdateDots;
        }
    }

    private void ShowWarning()
    {
        StopAllCoroutines();
        StartCoroutine(WarningRoutine());
    }

    private IEnumerator WarningRoutine()
    {
        warningRoot.SetActive(true);
        yield return new WaitForSeconds(warningDisplayTime);
        warningRoot.SetActive(false);
    }

    private void UpdateDots(int currentWave)
    {
        for (int i = 0; i < dots.Count; i++)
        {
            int waveNumber = i + 1;
            if (waveNumber < currentWave) dots[i].color = doneColor;
            else if (waveNumber == currentWave) dots[i].color = currentColor;
            else dots[i].color = pendingColor;
        }
    }
}