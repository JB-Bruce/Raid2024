using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonTracker : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject lastSelectedGameObject;
    private GameObject _currentSelectedGameObject_Recent;


    public static MenuButtonTracker instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        GetLastGameObjectSelected();
    }

    private void GetLastGameObjectSelected() // Gets the Last Selected Button
    {
        if (eventSystem.currentSelectedGameObject != _currentSelectedGameObject_Recent)
        {
            if (_currentSelectedGameObject_Recent != null)
            {
                // Trigger "Normal" animation on the previously selected button
                Animator oldAnimator = _currentSelectedGameObject_Recent.GetComponent<Animator>();
                if (oldAnimator != null)
                {
                    oldAnimator.SetTrigger("Normal");
                }
            }

            lastSelectedGameObject = _currentSelectedGameObject_Recent;
            _currentSelectedGameObject_Recent = eventSystem.currentSelectedGameObject;

            if (_currentSelectedGameObject_Recent == null)
            {
                AssignFirstButtonAsSelected();
            }
            else
            {
                // Trigger "Select" animation on the newly selected button
                Animator newAnimator = _currentSelectedGameObject_Recent.GetComponent<Animator>();
                if (newAnimator != null)
                {
                    newAnimator.SetTrigger("Selected");
                }
            }
        }
    }

    public void SetLastGameObjectSelected(GameObject newSelected) // Sets the EventSystem selected gameObject to a defined one
    {
        if (lastSelectedGameObject != null)
        {
            // Trigger "Normal" animation on the previously selected button
            Animator oldAnimator = lastSelectedGameObject.GetComponent<Animator>();
            if (oldAnimator != null)
            {
                oldAnimator.SetTrigger("Normal");
            }
        }

        lastSelectedGameObject = newSelected;
        eventSystem.SetSelectedGameObject(newSelected);

        // Trigger "Select" animation on the newly selected button
        Animator newAnimator = newSelected.GetComponent<Animator>();
        if (newAnimator != null)
        {
            newAnimator.SetTrigger("Selected");
        }
    }

    public void AssignFirstButtonAsSelected() // Sets the EventSystem selected gameObject to the first button it can find
    {
        Button firstFoundButton = FindObjectOfType<Button>();

        if (firstFoundButton != null)
        {
            GameObject firstButtonGameObject = firstFoundButton.gameObject;
            SetLastGameObjectSelected(firstButtonGameObject);
        }
        else
        {
            Debug.LogWarning("No Button found in scene to assign");
        }
    }
}
