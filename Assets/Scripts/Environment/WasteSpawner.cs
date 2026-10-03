using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class WasteSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private GameObject wasteItemPrefab;
    [SerializeField] private int wavesCount = 4;
    [SerializeField] private int itemsPerWave = 8;
    [SerializeField] private float waveInterval = 150f;

    private List<Vector3Int> waterCells;

    public int CurrentWave { get; private set; }
    public int TotalWaves => wavesCount;
    public event System.Action OnWaveIncoming;
    public event System.Action<int> OnWaveStarted;
    [SerializeField] private float warningLeadTime = 8f;

    private void Awake()
    {
        CacheWaterCells();
    }
    private void Start()
    {
        CurrentWave = 1;
        SpawnWave(itemsPerWave);
        NotifyPollution();
        OnWaveStarted?.Invoke(CurrentWave);
        StartCoroutine(WaveRoutine());
    }

    private void CacheWaterCells()
    {
        waterCells = new List<Vector3Int>();
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) == waterTile)
            {
                waterCells.Add(pos);
            }
        }
    }

    private IEnumerator WaveRoutine()
    {
        for (int i = 1; i < wavesCount; i++)
        {
            float waitBeforeWarning = Mathf.Max(0f, waveInterval - warningLeadTime);
            yield return new WaitForSeconds(waitBeforeWarning);

            OnWaveIncoming?.Invoke();
            yield return new WaitForSeconds(Mathf.Min(warningLeadTime, waveInterval));

            SpawnWave(itemsPerWave);
            CurrentWave++;
            NotifyPollution();
            OnWaveStarted?.Invoke(CurrentWave);
        }
    }

    private void SpawnWave(int count)
    {
        List<Vector3Int> pool = new List<Vector3Int>(waterCells);

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            Vector3 worldPos = tilemap.GetCellCenterWorld(pool[index]);
            pool.RemoveAt(index);
            Instantiate(wasteItemPrefab, worldPos, Quaternion.identity);
        }

        if (WaterTileEffect.Instance != null)
        {
            WaterTileEffect.Instance.RefreshTargets();
        }
    }

    private void NotifyPollution()
    {
        if (PollutionManager.Instance != null)
        {
            PollutionManager.Instance.RecalculateBaseline();
        }
    }
}