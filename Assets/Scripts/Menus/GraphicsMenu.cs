using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsMenu : MonoBehaviour
{
    [Header("References: ")]
    public GameObject graphicsMenu;
    public Toggle fullscreenToggle, vSyncToggle;

    public List<ResItem> resolutions = new List<ResItem>();

    public TMP_Text resText;

    private int _selectedRes;

    public static GraphicsMenu instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            gameObject.SetActive(false);
        }

        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0 )
        {
            vSyncToggle.isOn = false;
        }
        else
        {
            vSyncToggle.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                _selectedRes = i;

                UpdateResText();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);

            _selectedRes = resolutions.Count - 1;

            UpdateResText();
        }
    }

    private void Update()
    {
        
    }

    public void ResLeft()
    {
        _selectedRes--;
        if ( _selectedRes < 0 ) 
        { 
            _selectedRes = 0;
        }

        UpdateResText();
    }

    public void ResRight()
    {
        _selectedRes++;
        if ( _selectedRes > resolutions.Count - 1 ) 
        { 
            _selectedRes = resolutions.Count - 1;
        }

        UpdateResText();
    }

    public void UpdateResText()
    {
        resText.text = resolutions[_selectedRes].horizontal.ToString() + "x" + resolutions[_selectedRes].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenToggle.isOn;

        if (vSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[_selectedRes].horizontal, resolutions[_selectedRes].vertical, fullscreenToggle.isOn);
    }

    public void DeactivateGraphicsMenu()
    {
        graphicsMenu.SetActive(false);
        SettingsMenu.instance.isInSettings = false;
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
