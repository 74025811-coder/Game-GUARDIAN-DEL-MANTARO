using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CropController : MonoBehaviour
{
    [SerializeField] private Color wiltedColor = new Color(0.55f, 0.4f, 0.2f);
    [SerializeField] private Color healthyColor = new Color(0.1f, 0.55f, 0.15f);

    // Velocidad de la transición de color (más bajo = más lenta)
    [SerializeField] private float smoothSpeed = 0.5f;

    private SpriteRenderer sr;
    private float health; // 0 = marchito, 1 = sano

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        health = 0f; // empieza marchito, coherente con el 100% de contaminación inicial
        ApplyColor();
    }

    private void Update()
    {
        if (PollutionManager.Instance == null) return;

        float target = 1f - PollutionManager.Instance.PollutionPercentage / 100f;
        health = Mathf.MoveTowards(health, target, smoothSpeed * Time.deltaTime);
        ApplyColor();
    }

    private void ApplyColor()
    {
        sr.color = Color.Lerp(wiltedColor, healthyColor, health);
    }
}