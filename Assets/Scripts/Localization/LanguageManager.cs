using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class LanguageManager : MonoBehaviour
{
    public string filePath = "Assets/Localization.csv";
    private Dictionary<string, Dictionary<string, string>> localizationData;

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
    }

    public void ChangeLanguage(string newLanguage)
    {
        selectedLanguage = newLanguage;
        PlayerPrefs.SetString("Language", newLanguage);
        languageChangedEvent.Invoke();
    }

    void LoadLocalizationData()
    {
        localizationData = new Dictionary<string, Dictionary<string, string>>();

        string[] lines = File.ReadAllLines(filePath);

        var headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            var fields = lines[i].Split(',');

            var key = fields[0];
            var translations = new Dictionary<string, string>();

            for (int j = 1; j < fields.Length; j++)
            {
                translations[headers[j]] = fields[j];
            }

            localizationData[key] = translations;
        }
    }

    public string GetText(string key, string language = "")
    {
        if (language == "") language = selectedLanguage;

        if (localizationData.ContainsKey(key) && localizationData[key].ContainsKey(language))
        {
            return localizationData[key][language];
        }
        return key;
    }
}