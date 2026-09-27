using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [SerializeField] private float timeLimitSeconds = 1200f; // 20 minutos

    private float timeRemaining;
    private bool gameEnded;

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
        PollutionManager.Instance.OnPollutionCleared += HandleVictory;
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
            PollutionManager.Instance.OnPollutionCleared -= HandleVictory;
        }
    }
}