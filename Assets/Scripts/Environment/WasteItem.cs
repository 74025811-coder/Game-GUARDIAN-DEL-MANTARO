using UnityEngine;

public class WasteItem : MonoBehaviour
{
    [SerializeField] private int pollutionValue = 5; // usaremos esto en el Bloque 5

    public void Collect()
    {
        // Por ahora solo lo destruimos.
        // En el Bloque 5 aquí vamos a avisarle al sistema de contaminación.
        Destroy(gameObject);
    }
}