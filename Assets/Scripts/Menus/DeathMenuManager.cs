using UnityEngine;
using UnityEngine.UI;

public class DeathMenuManager : MonoBehaviour
{
    public static DeathMenuManager instance;

    [Header("Menu objets: ")]
    [SerializeField] GameObject _deathMenu;

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
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (!_conditionsManager.IsAnyMenuOpen())
            {
                ActivateDeathMenu();
            }
        }
    }

    public void ActivateDeathMenu() //Activates the EndGame menu and pauses game
    {

        _deathMenu.SetActive(true);
        _conditionsManager.hasDied = true;

        Time.timeScale = 0f;
    }

    public void RespawnAtLocation() //Allows to respawn at location// //For now not implemented, just closes menu
    {
        _deathMenu.SetActive(false);
        _conditionsManager.hasDied = false;

        Time.timeScale = 1f;
    }
}
