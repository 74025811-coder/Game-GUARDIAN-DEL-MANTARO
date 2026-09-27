using UnityEngine;
using System;

public class PollutionManager : MonoBehaviour
{
    public static PollutionManager Instance { get; private set; }

    private int totalWaste;
    private int remainingWaste;

    public float PollutionPercentage => totalWaste == 0 ? 0 : (float)remainingWaste / totalWaste * 100f;

    public event Action<float> OnPollutionChanged;
    public event Action OnPollutionCleared;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Cuenta todos los residuos que existen en la escena al iniciar
        totalWaste = FindObjectsByType<WasteItem>(FindObjectsSortMode.None).Length;
        remainingWaste = totalWaste;
        OnPollutionChanged?.Invoke(PollutionPercentage);
    }

    public void RegisterWasteCollected()
    {
        remainingWaste = Mathf.Max(0, remainingWaste - 1);
        OnPollutionChanged?.Invoke(PollutionPercentage);

        if (remainingWaste == 0)
        {
            OnPollutionCleared?.Invoke();
        }
    }
}