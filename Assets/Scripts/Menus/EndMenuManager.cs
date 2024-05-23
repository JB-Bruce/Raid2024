using UnityEngine;
using UnityEngine.UI;

public class EndMenuManager : MonoBehaviour
{
    public static EndMenuManager instance;

    [Header("Menu objets: ")]
    [SerializeField] GameObject _endMenu;

    [Header("Booleans: ")]
    public bool gameHasEnded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update() //Temp Test 
    {
        if (Input.GetKeyUp(KeyCode.E)) 
        {
            ActivateEndGame();
        }
    }

    public void ActivateEndGame() //Activates the EndGame menu and pauses game
    {
        if (!PauseManager.instance.isPaused) 
        {
            _endMenu.SetActive(true);
            gameHasEnded = true;

            Time.timeScale = 0f;
        }
    }

    public void ContinueGame() //Allows to continue the game and unpauses the game
    {
        if (!PauseManager.instance.isPaused)
        {
            _endMenu.SetActive(false);
            gameHasEnded = false;

            Time.timeScale = 1f;
        }
    }
}
