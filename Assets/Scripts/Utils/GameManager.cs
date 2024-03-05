#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using SOGameEventSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    bool isPaused;
    public bool IsPaused { get { return isPaused; } }
    bool gameStarted;
    public bool GameStarted { get { return gameStarted; } }

    public BaseGameEvent gamePauseEvent;
    public BaseGameEvent gameResumeEvent;
    public BaseGameEvent gameStartEvent;

    Controls.AppActions controls;

    private void Start()
    {
        controls = new Controls().App;

        controls.Pause.performed += ctx =>
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        };

        controls.Enable();
    }

    private void OnEnable()
    {
        if (controls.Equals(default(Controls.AppActions)))
            return;

        controls.Enable();
    }

    private void OnDisable()
    {
        if (controls.Equals(default(Controls.AppActions)))
            return;

        controls.Disable();
    }

    public void PauseGame()
    {
        if (!gameStarted)
            return;

        isPaused = true;
        Time.timeScale = 0f;
        gamePauseEvent.Raise();
    }

    public void ResumeGame()
    {
        if (!gameStarted)
            return;

        isPaused = false;
        Time.timeScale = 1f;
        gameResumeEvent.Raise();
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;
        gameStartEvent.Raise();
    }

    public void ReloadGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        // Quit the play mode in the editor
        EditorApplication.isPlaying = false;
#else
        // Quit the standalone application
        Application.Quit();
#endif
    }
}
