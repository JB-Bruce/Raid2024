using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LanguageManager : MonoBehaviour
{
    public string filePath = "Assets/Localization.csv";
    private Dictionary<string, Dictionary<string, string>> localizationData;

    public List<string> differentLanguages = new();
    public List<LanguageFlags> languageFlags = new();
    int languageIndex = 0;

    public Image flagImg;

    string selectedLanguage;

    public UnityEvent languageChangedEvent = new();

    public static LanguageManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        selectedLanguage = PlayerPrefs.GetString("Language", "French");
        LoadLocalizationData();
        InitializeFlag();
    }

    private void InitializeFlag()
    {
        for (int i = 0; i < differentLanguages.Count; i++)
        {
            if (differentLanguages[i] == selectedLanguage)
            {
                languageIndex = i;
                flagImg.sprite = GetFlagSprite(selectedLanguage);
                return;
            }
        }
    }

    public void NextLanguage()
    {
        languageIndex = (languageIndex + 1) % differentLanguages.Count;
        ChangeLanguage(differentLanguages[languageIndex]);
    }

    public void PreviousLanguage()
    {
        languageIndex = languageIndex - 1 < 0 ? differentLanguages.Count - 1 : languageIndex - 1;
        ChangeLanguage(differentLanguages[languageIndex]);
    }

    public void ChangeLanguage(string newLanguage)
    {
        if(flagImg != null)
        {
            flagImg.sprite = GetFlagSprite(newLanguage);
        }
        if(instance != this)
        {
            instance.ChangeLanguage(newLanguage);
            return;
        }
        selectedLanguage = newLanguage;
        PlayerPrefs.SetString("Language", newLanguage);
        languageChangedEvent.Invoke();
    }

    private Sprite GetFlagSprite(string language)
    {
        foreach (var item in languageFlags)
        {
            if (item.language == language) return item.sp;
        }
        return null;
    }

    void LoadLocalizationData()
    {

        localizationData = new Dictionary<string, Dictionary<string, string>>();

        string[] lines = File.ReadAllLines(filePath);

        var headers = lines[0].Split(';');

        for (int k = 1; k < headers.Length; k++)
        {
            differentLanguages.Add(headers[k]);
        }

        for (int i = 1; i < lines.Length; i++)
        {
            var fields = lines[i].Split(';');

            var key = fields[0];
            var translations = new Dictionary<string, string>();

            for (int j = 1; j < fields.Length; j++)
            {
                if(j > fields.Length - 1)
                {

                }
                if(j > headers.Length - 1)
                {

                }
                translations[headers[j]] = fields[j];
            }

            localizationData[key] = translations;
        }
    }

    public string GetText(string key, string language = "")
    {
        if(instance != this) return instance.GetText(key, language);

        if (language == "") language = selectedLanguage;

        if (localizationData.ContainsKey(key) && localizationData[key].ContainsKey(language))
        {
            return localizationData[key][language];
        }
        return key;
    }
}

[System.Serializable]
public struct LanguageFlags
{
    public string language;
    public Sprite sp;
}