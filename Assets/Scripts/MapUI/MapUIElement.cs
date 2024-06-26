using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUIElement : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    public void Init(Sprite _image, string _name, float _scale, Color _color)
    {
        image.sprite = _image;
        text.text = LanguageManager.instance.GetText(_name);
        image.transform.localScale = Vector3.one * _scale;
        image.color = _color;
        SetTextActive(false);
    }

    public void SetTextActive(bool newActive)
    {
        text.gameObject.SetActive(newActive);
    }
}
