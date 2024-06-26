using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonInputHandler : MonoBehaviour
{
    // Reference to the button you want to click programmatically
    public Button targetButton;
    [SerializeField] private PlayerInput _playerInput;

    private void Awake()
    {
        GameObject inactiveObject = FindInactiveObjectByName("SettingsBackButton");
        targetButton = inactiveObject.GetComponent<Button>();
        _playerInput = GetComponent<PlayerInput>();
    }

    public static GameObject FindInactiveObjectByName(string name)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.hideFlags == HideFlags.None && t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    // Method to handle the input action's callback
    public void OnClickActionPerformed()
    {
        // Invoke the button's onClick method
        if (targetButton != null)
        {
            targetButton.onClick.Invoke();
            _playerInput.SwitchCurrentActionMap("Pause");
        }
    }
}
