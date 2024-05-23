using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [Header("Menu objets: ")]
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Image _pauseMenuBackgroundImage;

    [Header("Booleans: ")]
    public bool isPaused = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update() //Temp Test
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else if (isPaused && !SettingsMenu.instance.isInSettings)
            {
                UnpauseGame();
            }
        }
    }

    private void OnApplicationPause(bool pause) //Pauses the game on Application quit
    {
        if (pause && !EndMenuManager.instance.gameHasEnded)
        {
            PauseGame();
        }
    }

    public void PauseGame() //Pauses the game
    {
        if (!EndMenuManager.instance.gameHasEnded)
        {
            _pauseMenu.SetActive(true);
            _pauseMenuBackgroundImage.enabled = true;
            isPaused = true;

            Time.timeScale = 0f;
        }
    }

    public void UnpauseGame() //Unpauses the game
    {
        if (!EndMenuManager.instance.gameHasEnded)
        {
            _pauseMenu.SetActive(false);
            _pauseMenuBackgroundImage.enabled = false;
            isPaused = false;

            Time.timeScale = 1f;
        }
    }
}
