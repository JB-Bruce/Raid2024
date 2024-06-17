using TMPro;
using UnityEngine;

public class LocalizationSync : MonoBehaviour
{
    public string text;
    public string language;
    private TextMeshProUGUI uiText;
    private LanguageManager languageManager;

    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        LanguageManager.instance.languageChangedEvent.AddListener(UpdateText);
        UpdateText();
    }

    public void UpdateText()
    {
        uiText.text = LanguageManager.instance.GetText(text);
    }
}
