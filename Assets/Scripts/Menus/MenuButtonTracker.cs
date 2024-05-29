using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonTracker : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject lastSelectedGameObject;
    private GameObject _currentSelectedGameObject_Recent;

    void Update()
    {
        GetLastGameObjectSelected();
    }


    private void GetLastGameObjectSelected() //Gets the Last Selected Button
    {
        if (eventSystem.currentSelectedGameObject != _currentSelectedGameObject_Recent)
        {
            lastSelectedGameObject = _currentSelectedGameObject_Recent;
            _currentSelectedGameObject_Recent = eventSystem.currentSelectedGameObject;

            if (_currentSelectedGameObject_Recent == null) 
            {
                AssignFirstButtonAsSelected();
            }
        }
    }

    public void SetLastGameObjectSelected(GameObject newSelected) //Sets the EventSystem selected gameObject to a define one
    {
        lastSelectedGameObject = newSelected;
        eventSystem.SetSelectedGameObject(newSelected);
    }

    public void AssignFirstButtonAsSelected() //Sets the EventSystem selected gameObject to the first button it can find
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
