using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WaterTileEffect : MonoBehaviour
{
    public static WaterTileEffect Instance { get; private set; }

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private Color dirtyColor = new Color(0.2f, 0.3f, 0.25f);
    [SerializeField] private Color cleanColor = new Color(0.3f, 0.6f, 0.9f);
    [SerializeField] private float influenceRadius = 10f;   // alcance de la suciedad de cada residuo (en tiles)
    [Range(0f, 1f)]
    [SerializeField] private float backgroundDirt = 0.6f;   // suciedad general del agua al inicio
    [SerializeField] private float fadeSpeed = 1f;          // velocidad de la transición

    private readonly List<Vector3Int> waterCells = new List<Vector3Int>();
    private float[] current;
    private float[] target;
    private int initialWaste;
    private bool animating;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) == waterTile)
            {
                waterCells.Add(pos);
            }
        }

        current = new float[waterCells.Count];
        target = new float[waterCells.Count];
        initialWaste = Mathf.Max(1, WasteItem.Active.Count);

        ComputeTargets();

        // Estado inicial sin animación
        for (int i = 0; i < current.Length; i++)
        {
            current[i] = target[i];
            ApplyColor(i);
        }
    }

    public void RefreshTargets()
    {
        ComputeTargets();
        animating = true;
    }

    private void ComputeTargets()
    {
        float ratio = (float)WasteItem.Active.Count / initialWaste;

        List<Vector3Int> wasteCells = new List<Vector3Int>();
        foreach (var waste in WasteItem.Active)
        {
            wasteCells.Add(tilemap.WorldToCell(waste.transform.position));
        }

        for (int i = 0; i < waterCells.Count; i++)
        {
            float nearest = float.MaxValue;
            Vector2 cellPos = new Vector2(waterCells[i].x, waterCells[i].y);

            foreach (var wc in wasteCells)
            {
                float d = Vector2.Distance(cellPos, new Vector2(wc.x, wc.y));
                if (d < nearest) nearest = d;
            }

            float proximityDirt = Mathf.Clamp01(1f - nearest / influenceRadius);
            target[i] = Mathf.Max(backgroundDirt * ratio, proximityDirt);
        }
    }

    private void Update()
    {
        if (!animating) return;

        bool pending = false;

        for (int i = 0; i < current.Length; i++)
        {
            if (Mathf.Approximately(current[i], target[i])) continue;

            current[i] = Mathf.MoveTowards(current[i], target[i], fadeSpeed * Time.deltaTime);
            ApplyColor(i);

            if (!Mathf.Approximately(current[i], target[i])) pending = true;
        }

        animating = pending;
    }

    private void ApplyColor(int i)
    {
        Vector3Int cell = waterCells[i];
        tilemap.SetTileFlags(cell, TileFlags.None);
        tilemap.SetColor(cell, Color.Lerp(cleanColor, dirtyColor, current[i]));
    }
}