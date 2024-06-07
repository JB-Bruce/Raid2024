using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtonLink : MonoBehaviour
{
    [SerializeField] GameObject _settingsMenus;

    private void Start()
    {
        _settingsMenus = SettingsMenusManager.instance.gameObject;
    }

    public void ActivateSettingsMenus()
    {
        _settingsMenus.SetActive(true);
        SettingsMenusManager.instance.isInSettings = true;
    }
}
