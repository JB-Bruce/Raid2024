using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsManager : MonoBehaviour
{
    public static ConditionsManager instance;

    [Header("Booleans: ")]
    public bool isPaused = false;
    public bool gameHasEnded = false;
    public bool hasDied = false;

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

    public bool IsAnyMenuOpen() //Checks if any of the menus is open
    {
        return isPaused || hasDied || gameHasEnded;
    }
}
