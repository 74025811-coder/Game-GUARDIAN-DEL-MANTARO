using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [SerializeField] private float timeLimitSeconds = 1200f; // 20 minutos

    private float timeRemaining;
    private bool gameEnded;
    private WasteSpawner spawner; // Referencia añadida para comprobar las oleadas

    public event Action<float> OnTimeChanged;
    public event Action OnVictory;
    public event Action OnDefeat;

    private void Awake()
    {
        Instance = this;
        timeRemaining = timeLimitSeconds;
    }

    private void Start()
    {
        spawner = FindFirstObjectByType<WasteSpawner>();
        PollutionManager.Instance.OnPollutionCleared += HandlePollutionCleared;
    }

    private void Update()
    {
        if (gameEnded) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            OnTimeChanged?.Invoke(timeRemaining); // notifica el 00:00 correcto
            HandleTimeUp();
            return;
        }

        OnTimeChanged?.Invoke(timeRemaining);
    }

    private void HandleTimeUp()
    {
        if (gameEnded) return;

        if (PollutionManager.Instance.PollutionPercentage <= 0f)
        {
            HandleVictory();
        }
        else
        {
            HandleDefeat();
        }
    }

    private void HandlePollutionCleared()
    {
        if (gameEnded) return;

        // Verifica si ya no quedan más oleadas antes de declarar la victoria
        bool isLastWave = spawner == null || spawner.CurrentWave >= spawner.TotalWaves;
        if (isLastWave)
        {
            HandleVictory();
        }
        // Si no es la última oleada, no pasa nada y el juego continúa.
    }

    private void HandleVictory()
    {
        if (gameEnded) return;
        gameEnded = true;
        OnVictory?.Invoke();
    }

    private void HandleDefeat()
    {
        if (gameEnded) return;
        gameEnded = true;
        OnDefeat?.Invoke();
    }

    private void OnDisable()
    {
        if (PollutionManager.Instance != null)
        {
            PollutionManager.Instance.OnPollutionCleared -= HandlePollutionCleared;
        }
    }
}