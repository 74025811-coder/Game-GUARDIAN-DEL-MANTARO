using UnityEngine;
using System.Collections.Generic;

public class WasteItem : MonoBehaviour
{
    public static readonly List<WasteItem> Active = new List<WasteItem>();

    [SerializeField] private int pressesRequired = 5;

    private int currentPresses;

    public float Progress01 => (float)currentPresses / pressesRequired;
    public bool IsBeingCollected => currentPresses > 0;

    private void OnEnable() => Active.Add(this);
    private void OnDisable() => Active.Remove(this);

    public void RegisterPress()
    {
        currentPresses = Mathf.Min(currentPresses + 1, pressesRequired);

        if (currentPresses >= pressesRequired)
        {
            Collect();
        }
    }

    public void ResetProgress()
    {
        currentPresses = 0;
    }

    private void Collect()
    {
        Active.Remove(this);

        PollutionManager.Instance.RegisterWasteCollected();

        if (WaterTileEffect.Instance != null)
        {
            WaterTileEffect.Instance.RefreshTargets();
        }

        Destroy(gameObject);
    }
}