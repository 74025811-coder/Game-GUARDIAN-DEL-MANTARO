using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void LoadGame() => SceneManager.LoadScene("Game");
    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
    public void QuitGame() => Application.Quit();
}