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

    [Header("Lists: ")]
    public List<ResItem> resolutions = new List<ResItem>();
    public List<int> fpsLimits = new List<int>();

    public TMP_Text resText;
    public TMP_Text fpsLimitText;

    private int _selectedRes;
    private int _selectedFPSLimit;

    public int currentFPS;

    public string unlimitedFPSLimit;

    public static GraphicsMenu instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
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
        FindCurrentRes();
    }

    private void FindCurrentRes()
    {
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
        currentFPS = Application.targetFrameRate;
    }

    public void ResLeft()
    {
        _selectedRes--;
        if ( _selectedRes < 0 ) 
        {
            _selectedRes = resolutions.Count - 1;
        }

        UpdateResText();
    }

    public void ResRight()
    {
        _selectedRes++;
        if ( _selectedRes > resolutions.Count - 1 ) 
        { 
            _selectedRes = 0;
        }

        UpdateResText();
    }

    public void FPSLimitLeft()
    {
        _selectedFPSLimit--;
        if (_selectedFPSLimit < 0)
        {
            _selectedFPSLimit = fpsLimits.Count - 1;
        }

        UpdateFPSLimitText();
    }

    public void FPSLimitRight()
    {
        _selectedFPSLimit++;
        if (_selectedFPSLimit > fpsLimits.Count - 1)
        {
            _selectedFPSLimit = 0;
        }

        UpdateFPSLimitText();
    }

    public void UpdateResText()
    {
        resText.text = resolutions[_selectedRes].horizontal.ToString() + "x" + resolutions[_selectedRes].vertical.ToString();
    }

    public void UpdateFPSLimitText()
    {
        if (_selectedFPSLimit >= 2)
        {
            fpsLimitText.text = unlimitedFPSLimit;
        }
        else if (_selectedFPSLimit <= 1)
        {
            fpsLimitText.text = fpsLimits[_selectedFPSLimit].ToString();
        }
        
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
        Application.targetFrameRate = fpsLimits[_selectedFPSLimit];
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
