using UnityEngine;
using System.Collections.Generic;

public class WasteItem : MonoBehaviour
{
    public static readonly List<WasteItem> Active = new List<WasteItem>();

    private void OnEnable() => Active.Add(this);
    private void OnDisable() => Active.Remove(this);

    public void Collect()
    {
        Active.Remove(this); // sale de la lista ANTES de recalcular el agua

        PollutionManager.Instance.RegisterWasteCollected();

        if (WaterTileEffect.Instance != null)
        {
            WaterTileEffect.Instance.RefreshTargets();
        }

        Destroy(gameObject);
    }
}