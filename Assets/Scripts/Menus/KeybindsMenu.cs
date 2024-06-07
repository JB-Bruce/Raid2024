using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindsMenu : MonoBehaviour
{
    [Header("References: ")]
    public GameObject keybindsMenu;

    public static KeybindsMenu instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
