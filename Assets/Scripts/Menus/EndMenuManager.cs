using UnityEngine;
using UnityEngine.UI;

public class EndMenuManager : MonoBehaviour
{
    public static EndMenuManager instance;

    [Header("Menu objets: ")]
    [SerializeField] GameObject _endMenu;

    ConditionsManager _conditionsManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _conditionsManager = ConditionsManager.instance;
    }

    private void Update() //Temp Test 
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (!_conditionsManager.IsAnyMenuOpen())
            {
                ActivateEndGame();
            }
        }
    }

    public void ActivateEndGame() //Activates the EndGame menu and pauses game
    {
        _endMenu.SetActive(true);
        _conditionsManager.gameHasEnded = true;

        Time.timeScale = 0f;
    }

    public void ContinueGame() //Allows to continue the game and unpauses the game
    {
        _endMenu.SetActive(false);
        _conditionsManager.gameHasEnded = false;

        Time.timeScale = 1f;
    }
}
