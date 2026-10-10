using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public static bool IsPaused { get; private set; }

    private void Start()
    {
        Resume(); // deja el juego en estado normal al iniciar
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (IsPaused) Resume(); else Pause();
        }
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
}