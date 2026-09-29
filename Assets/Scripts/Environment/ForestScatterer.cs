using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ForestScatterer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase forestTile;
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject[] bushPrefabs;
    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField, Range(0f, 1f)] private float density = 0.45f;
    [SerializeField] private Transform parent;

    private void Awake()
    {
        List<Vector3Int> forestCells = new List<Vector3Int>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) == forestTile)
            {
                forestCells.Add(pos);
            }
        }

        foreach (var cell in forestCells)
        {
            if (Random.value > density) continue;

            GameObject prefab = PickPrefab();
            if (prefab == null) continue;

            Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
            worldPos += new Vector3(Random.Range(-0.35f, 0.35f), Random.Range(-0.35f, 0.35f), 0f);

            GameObject instance = Instantiate(prefab, worldPos, Quaternion.identity, parent);
            float scale = Random.Range(0.9f, 1.15f);
            instance.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    private GameObject PickPrefab()
    {
        float r = Random.value;
        if (r < 0.55f && treePrefabs.Length > 0) return treePrefabs[Random.Range(0, treePrefabs.Length)];
        if (r < 0.85f && bushPrefabs.Length > 0) return bushPrefabs[Random.Range(0, bushPrefabs.Length)];
        if (rockPrefabs.Length > 0) return rockPrefabs[Random.Range(0, rockPrefabs.Length)];
        return null;
    }
}