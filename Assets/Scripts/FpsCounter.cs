using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = (fps).ToString("F1");

        if(Input.GetKeyDown(KeyCode.F1)) fpsText.gameObject.SetActive(!fpsText.gameObject.activeInHierarchy);
    }
}
