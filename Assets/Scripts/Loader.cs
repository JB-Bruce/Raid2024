using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private string _sceneName;
    [SerializeField] private float _waitTime;
    [SerializeField] private PlayerInput _playerInput;

    private void Start()
    {

        StartCoroutine(LoadScene());
    }

    // Coroutine For LoadScene
    private IEnumerator LoadScene()
    {
        float timer = Time.time + _waitTime;
        float startTimer = Time.time;
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
        while (Time.time < _waitTime)
        {
            SetSlider((Time.time - startTimer) * 100 / _waitTime);
            yield return null;
        }
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    // Set the scale of the slider
    private void SetSlider(float value)
    {
        value = Mathf.Clamp01(value / 100);
        _rectTransform.localScale = new Vector3(value, 1, 1);
    }
}
