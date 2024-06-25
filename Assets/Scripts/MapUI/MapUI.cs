using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapUI : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    public Transform top;
    public Transform bot;
    public Transform right;
    public Transform left;

    public Transform UITop;
    public Transform UIBot;
    public Transform UIRight;
    public Transform UILeft;

    List<GameObject> _list = new();

    List<(Transform, Transform)> _movingMapElements = new();

    public GameObject prefab;
    public Transform parent;

    Vector3 _ratioVertical;
    Vector3 _ratioVerticalUI;
    Vector3 _ratioHorizontal;
    Vector3 _ratioHorizontalUI;

    bool _isOpen;

    public GameObject mapUI;

    public static MapUI instance;

    public bool isMapOpen = false;

    private void Awake()
    {
        instance = this;
        _ratioVertical = bot.transform.position - top.transform.position;
        _ratioVerticalUI = UIBot.transform.position - UITop.transform.position;

        _ratioHorizontal = left.transform.position - right.transform.position;
        _ratioHorizontalUI = UILeft.transform.position - UIRight.transform.position;
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
    public void SetElementToMap(Vector3 pos, Sprite sp, Color c, float size, string _name)
    {
        GameObject go = Instantiate(prefab, parent);

        Vector3 newPos = Vector3.zero;

        newPos.y = UIBot.position.y + (pos.y - bot.position.y) * ((UITop.position.y - UIBot.position.y) / (top.position.y - bot.position.y));
        newPos.x = UILeft.position.x + (pos.x - left.position.x) * ((UIRight.position.x - UILeft.position.x) / (right.position.x - left.position.x));

        go.GetComponent<MapUIElement>().Init(sp, _name, size, c);

        _list.Add(go);

        go.transform.position = newPos;
    }

    // Put moving element on the UI
    public void SetMovingElementToMap(Transform t, Sprite sp, Color c, float size, string _name)
    {
        GameObject go = Instantiate(prefab, parent);
        go.GetComponent<MapUIElement>().Init(sp, _name, size, c);
        _movingMapElements.Add((t, go.transform));
    }

    public Vector2 GetWorldPosFromUIPos(Vector2 pos)
    {
        Vector3 newPos = Vector3.zero;

        newPos.y = bot.position.y + (pos.y - UIBot.position.y) * ((top.position.y - bot.position.y) / (UITop.position.y - UIBot.position.y));
        newPos.x = left.position.x + (pos.x - UILeft.position.x) * ((right.position.x - left.position.x) / (UIRight.position.x - UILeft.position.x));

        return newPos;
    }

    public Vector2 GetUIPosFromWorldPos(Vector2 pos)
    {
        Vector3 newPos = Vector3.zero;

        newPos.y = UIBot.position.y + (pos.y - bot.position.y) * ((UITop.position.y - UIBot.position.y) / (top.position.y - bot.position.y));
        newPos.x = UILeft.position.x + (pos.x - left.position.x) * ((UIRight.position.x - UILeft.position.x) / (right.position.x - left.position.x));

        return newPos;
    }

    // Update all the moving elements
    private void Update()
    {
        foreach (var t in _movingMapElements)
        {
            Vector3 newPos = Vector3.zero;
            Vector3 pos = t.Item1.position;

            newPos.y = UIBot.position.y + (pos.y - bot.position.y) * ((UITop.position.y - UIBot.position.y) / (top.position.y - bot.position.y));
            newPos.x = UILeft.position.x + (pos.x - left.position.x) * ((UIRight.position.x - UILeft.position.x) / (right.position.x - left.position.x));

            t.Item2.position = newPos;
        }
    }

    
}
