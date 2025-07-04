using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameManager gameManager;

    public void Play()
    {
        mainMenu.SetActive(false);
        gamePanel.SetActive(true);
        gameManager.InitializeGame();
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Defeat()
    {
        mainMenu.SetActive(true);

    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
