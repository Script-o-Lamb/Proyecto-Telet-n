using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // El panel del men� de pausa
    [SerializeField] private string finalSceneName = "FinalScene"; // nombre de la escena final

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.GuardarPuntajeFinal();
        }

        // Aseg�rate de reiniciar el tiempo por si estaba en pausa
        Time.timeScale = 1f;

        // Cargar la escena final
        SceneManager.LoadScene(finalSceneName);
    }
}
