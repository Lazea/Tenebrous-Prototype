using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject gameplayPanel;
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;

    public void ShowGameplayPanel()
    {
        gameplayPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    public void ShowMainMenuPanel()
    {
        gameplayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    public void ShowPauseMenuPanel()
    {
        gameplayPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
}
