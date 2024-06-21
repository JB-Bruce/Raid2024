using UnityEngine;
using UnityEngine.InputSystem;

public class RansomPaper : MonoBehaviour
{
    [SerializeField]
    private GameObject _ransomPaper;

    [SerializeField]
    private PlayerInput _playerInput;

    public static RansomPaper instance;

    //create an instance of the RansomPaper
    private void Awake()
    {
        instance = this;
    }

    //close the ransom paper
    public void CloseRansomPaper()
    {
        _playerInput.SwitchCurrentActionMap("InGame");
        Time.timeScale = 1f;
        _ransomPaper.SetActive(false);
    }

    //open the ransom paper
    public void OpenRansomPaper()
    {
        _ransomPaper.SetActive(true);
        Time.timeScale = 0f;
        _playerInput.SwitchCurrentActionMap("RansomPaper");
    }
}