using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WasteSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private GameObject wasteItemPrefab;
    [SerializeField] private int wasteCount = 15;

    private void Awake()
    {
        List<Vector3Int> waterCells = new List<Vector3Int>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) == waterTile)
            {
                waterCells.Add(pos);
            }
        }

        for (int i = 0; i < wasteCount && waterCells.Count > 0; i++)
        {
            int index = Random.Range(0, waterCells.Count);
            Vector3 worldPos = tilemap.GetCellCenterWorld(waterCells[index]);
            waterCells.RemoveAt(index); // evita dos residuos en el mismo tile

            Instantiate(wasteItemPrefab, worldPos, Quaternion.identity);
        }
    }
}