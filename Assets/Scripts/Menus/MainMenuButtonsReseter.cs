using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonsReseter : MonoBehaviour
{

    public static MainMenuButtonsReseter instance;

    [SerializeField] List<GameObject> _buttons = new List<GameObject>();

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            SetStateButtons();
        }
    }
    private void OnEnable()
    {
        ResetButtonAnimations();
    }

    private void SetStateButtons()
    {
        foreach (GameObject button in _buttons)
        {
            Animator animator = button.GetComponent<Animator>();
            if (animator != null)
            {
                animator.keepAnimatorStateOnDisable = true;
            }
        }
    }

    public void ResetButtonAnimations() // Resets animations of all buttons
    {
        foreach (GameObject button in _buttons)
        {
            Animator animator = button.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Normal", 0, 0f);
            }
        }
    }
}
