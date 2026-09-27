using UnityEngine;

public class WasteItem : MonoBehaviour
{
    [SerializeField] private int pollutionValue = 5; // lo usaremos si luego quieres pesos distintos

    public void Collect()
    {
        PollutionManager.Instance.RegisterWasteCollected();
        Destroy(gameObject);
    }
}