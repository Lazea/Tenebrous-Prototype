using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;

    [Header("First Event System Selections")]
    public GameObject pauseMenuDefaultSelect;
    public GameObject optionsMenuDefaultSelect;

    private void OnEnable()
    {
        ShowPauseMenu();
    }

    public void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(pauseMenuDefaultSelect);
    }

    public void ShowOptions()
    {
        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(optionsMenuDefaultSelect);
    }
}
