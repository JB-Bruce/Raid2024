using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    public Transform topRight;
    public Transform botLeft;

    public Transform UITopRight;
    public Transform UIBotLeft;

    List<GameObject> _list = new();

    List<(Transform, Transform)> _movingMapElements = new();

    public GameObject prefab;
    public Transform parent;

    Vector3 _ratio1;
    Vector3 _ratio2;

    bool _isOpen;

    public GameObject mapUI;

    public static MapUI Instance;

    public bool isMapOpen = false;

    private void Awake()
    {
        Instance = this;
        _ratio1 = botLeft.transform.position - topRight.transform.position;
        _ratio2 = UIBotLeft.transform.position - UITopRight.transform.position;
    }

    /// <summary>
    /// input action to open and close the map
    /// </summary>
    public void OpenCloseMapAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OpenCloseMap();
        }
    }

    // UI button to open or close
    public void OpenCloseMap()
    {
        if (_playerInput.actions.FindActionMap("InGame").enabled || _playerInput.actions.FindActionMap("Map").enabled)
        {
            mapUI.SetActive(!mapUI.activeInHierarchy);
            if (mapUI.activeInHierarchy)
            {
                isMapOpen = true;
                _playerInput.actions.FindActionMap("InGame").Disable();
                _playerInput.actions.FindActionMap("Map").Enable();
            }
            else
            {
                isMapOpen = false;
                _playerInput.actions.FindActionMap("Map").Disable();
                _playerInput.actions.FindActionMap("InGame").Enable();
            }
        }
    }


    // Put fix element on the UI 
    public void SetElementToMap(Vector3 pos, Sprite sp, Color c, float size)
    {
        GameObject go = Instantiate(prefab, parent);

        Vector3 newPos = Vector3.zero;

        newPos = UIBotLeft.position + (pos - botLeft.position) * ((UITopRight.position - UIBotLeft.position).magnitude / (topRight.position - botLeft.position).magnitude);

        go.GetComponent<Image>().sprite = sp;
        go.GetComponent<Image>().color = c;

        go.transform.localScale *= size;

        _list.Add(go);

        go.transform.position = newPos;
    }

    // Put moving element on the UI
    public void SetMovingElementToMap(Transform t, Sprite sp, Color c, float size)
    {
        GameObject go = Instantiate(prefab, parent);
        go.GetComponent<Image>().sprite = sp;
        go.GetComponent<Image>().color = c;
        go.transform.localScale *= size;
        _movingMapElements.Add((t, go.transform));
    }

    // Update all the moving elements
    private void Update()
    {
        foreach (var t in _movingMapElements)
        {
            Vector3 newPos = Vector3.zero;
            Vector3 pos = t.Item1.position;

            newPos = UIBotLeft.position + (pos - botLeft.position) * ((UITopRight.position - UIBotLeft.position).magnitude / (topRight.position - botLeft.position).magnitude);

            t.Item2.position = newPos;
        }
    }
}
