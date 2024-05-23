using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ChangeScene(string _sceneName)
    {
        //Loads the scene by its name
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(_sceneName);
    }

    public void QuitGame()
    {
        //Quits the game
        Application.Quit();
    }
}
