using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 1.2f;
    [SerializeField] private LayerMask interactableMask; // opcional, puedes dejarlo en Everything

    private WasteItem currentTarget;

    private void Update()
    {
        DetectNearbyWaste();

        if (currentTarget != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentTarget.Collect();
            currentTarget = null;
        }
    }

    private void DetectNearbyWaste()
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

        currentTarget = closest;
    }

    // Lo usaremos para mostrar/ocultar el texto "[E] Recoger"
    public bool HasTarget => currentTarget != null;
    public Transform CurrentTargetTransform => currentTarget != null ? currentTarget.transform : null;
}