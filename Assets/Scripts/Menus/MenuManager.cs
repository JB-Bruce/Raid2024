using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] PlayerInput _playerInput;

    public void OpenSettingsInGame(string actionMapToSwitchTo)
    {
        SettingsMenusManager.instance.OpenSettings(actionMapToSwitchTo);
    }

    public void CloseSettingsInGame(string actionMapToSwitchTo)
    {
        if (SettingsMenu.instance != null)
        {
            SettingsMenusManager.instance.DeactivateSettingsMenus(actionMapToSwitchTo);
        }
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
        SettingsMenusManager.instance.DeactivateSettingsMenus("InGame");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        //Quits the game
        Application.Quit();
    }
}
