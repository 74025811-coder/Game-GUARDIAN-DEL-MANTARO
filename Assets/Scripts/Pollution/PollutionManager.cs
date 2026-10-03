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

    // Se llama cada vez que llega una oleada nueva (incluida la primera).
    // Toma TODOS los residuos activos en ese momento (los sobrantes + los nuevos) como el 100% de esa etapa.
    public void RecalculateBaseline()
    {
        totalWaste = WasteItem.Active.Count;
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