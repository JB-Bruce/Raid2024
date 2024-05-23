using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [Header("Menu objets: ")]
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Image _pauseMenuBackgroundImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnApplicationPause(bool pause) //Pauses the game on Application quit
    {
        if (pause)
        {
            PauseGame();
        }
    }

    public void PauseGame() //Pauses the game
    {
        _pauseMenu.SetActive(true);
        _pauseMenuBackgroundImage.enabled = true;

        Time.timeScale = 0f;
    }

    public void UnpauseGame() //Unpauses the game
    {
        _pauseMenu.SetActive(false);
        _pauseMenuBackgroundImage.enabled = false;

        Time.timeScale = 1f;
    }
}
