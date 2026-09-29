using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 1.2f;

    private WasteItem currentTarget;

    public bool HasTarget => currentTarget != null;
    public Transform CurrentTargetTransform => currentTarget != null ? currentTarget.transform : null;
    public float CurrentProgress01 => currentTarget != null ? currentTarget.Progress01 : 0f;

    private void Update()
    {
        WasteItem detected = DetectNearest();

        // Si cambiaste de objetivo (o te alejaste de todos), resetea el progreso del anterior
        if (detected != currentTarget)
        {
            currentTarget?.ResetProgress();
            currentTarget = detected;
        }

        if (currentTarget != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentTarget.RegisterPress();
        }
    }

    private WasteItem DetectNearest()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        WasteItem closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            WasteItem waste = hit.GetComponent<WasteItem>();
            if (waste != null)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = waste;
                }
            }
        }

        return closest;
    }
}