using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtonLink : MonoBehaviour
{
    [SerializeField] GameObject _settingsMenu;

    private void Start()
    {
        _settingsMenu = SettingsMenu.instance.gameObject;
    }

    public void ActivateSettingsMenu()
    {
        _settingsMenu.SetActive(true);
        SettingsMenu.instance.isInSettings = true;
    }
}
