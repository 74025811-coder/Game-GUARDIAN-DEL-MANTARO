using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class WasteItem : MonoBehaviour
{
    public static readonly List<WasteItem> Active = new List<WasteItem>();

    [SerializeField] private int pressesRequired = 5;

    [Header("Audio")]
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioClip collectClip;

    [Header("Feedback visual")]
    [SerializeField, Range(0.2f, 1f)] private float minScale = 0.65f;
    [SerializeField] private float shakeAmount = 0.06f;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float popDuration = 0.15f;

    private SpriteRenderer sr;
    private Vector3 basePosition;
    private Vector3 baseScale;
    private int currentPresses;
    private bool collected;

    public float Progress01 => (float)currentPresses / pressesRequired;
    public bool IsBeingCollected => currentPresses > 0;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        basePosition = transform.position;
        baseScale = transform.localScale;
    }

    private void OnEnable() => Active.Add(this);
    private void OnDisable() => Active.Remove(this);

    public void RegisterPress()
    {
        if (collected) return;

        currentPresses = Mathf.Min(currentPresses + 1, pressesRequired);

        if (currentPresses >= pressesRequired)
        {
            Collect();
            return;
        }

        PlaySfx(pressClip);

        transform.localScale = baseScale * Mathf.Lerp(1f, minScale, Progress01);
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    public void ResetProgress()
    {
        if (collected) return;

        currentPresses = 0;
        StopAllCoroutines();
        transform.position = basePosition;
        transform.localScale = baseScale;
    }

    private void Collect()
    {
        collected = true;
        PlaySfx(collectClip);
        Active.Remove(this);

        // Desactiva el collider para que el jugador ya no lo detecte mientras desaparece
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        PollutionManager.Instance.RegisterWasteCollected();

        if (WaterTileEffect.Instance != null)
        {
            WaterTileEffect.Instance.RefreshTargets();
        }

        StopAllCoroutines();
        StartCoroutine(PopRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float t = 0f;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            transform.position = basePosition + (Vector3)(Random.insideUnitCircle * shakeAmount);
            yield return null;
        }
        transform.position = basePosition;
    }

    private IEnumerator PopRoutine()
    {
        transform.position = basePosition;
        Vector3 startScale = transform.localScale;
        Color startColor = sr.color;
        float t = 0f;

        while (t < popDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / popDuration);
            transform.localScale = startScale * Mathf.Lerp(1f, 1.4f, k);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f - k);
            yield return null;
        }

        Destroy(gameObject);
    }
    private void PlaySfx(AudioClip clip)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clip);
        }
    }
}