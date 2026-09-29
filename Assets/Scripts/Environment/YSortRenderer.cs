using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortRenderer : MonoBehaviour
{
    [SerializeField] private int baseOffset = 10000;
    [SerializeField] private int precision = 10;

    private void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = baseOffset - Mathf.RoundToInt(transform.position.y * precision);
    }
}