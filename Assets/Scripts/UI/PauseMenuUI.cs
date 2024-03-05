using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;

    private void OnEnable()
    {
        ShowPauseMenu();
    }

    public void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}
