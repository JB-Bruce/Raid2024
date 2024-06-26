using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    SettingsMenusManager _settingsMenus;
    MenuButtonTracker _menuButtonTracker;
    SoundManager _soundManager;

    public static MenuManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _settingsMenus = SettingsMenusManager.instance;
        _soundManager = SoundManager.instance;
    }

    public void OpenSettingsInGame()
    {
        _settingsMenus.OpenSettings();
    }

    public void CloseSettingsInGame()
    {
        if (SettingsMenu.instance != null)
        {
            _settingsMenus.DeactivateSettingsMenus();
        }
    }

    public void GetSelectedSettingsButton(string buttonToFind)
    {
        _menuButtonTracker = MenuButtonTracker.instance;

        _menuButtonTracker.SetLastGameObjectSelected(GameObject.Find(buttonToFind));
    }

    public void ChangeScene(string _sceneName)
    {
        //Loads the scene by its name
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(_sceneName);
    }

    public void ReturnToGame()
    {
        _playerInput.SwitchCurrentActionMap("InGame");
        this.gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        _soundManager.PlaySFX("ButtonClick", _soundManager._sfxPlayer);
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        //Quits the game
        Application.Quit();
    }
}
