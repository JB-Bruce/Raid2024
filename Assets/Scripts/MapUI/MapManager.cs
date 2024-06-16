using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    GameObject overedObject;

    public Transform playerPing;

    public Transform Top;
    public Transform Bottom;
    public Transform Left;
    public Transform Right;

    public Transform playerT;
    public Transform playerPingArrow;
    public Transform questPingArrow;

    public bool doesArrowsFollowPlayer;

    public float distanceToDesappear;

    public bool IsPointerOverUIElement(out GameObject uiElement)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            uiElement = results[0].gameObject;
            return true;
        }

        uiElement = null;
        return false;
    }

    private void Start()
    {
        playerPing.gameObject.SetActive(false);
        playerPingArrow.gameObject.SetActive(false);
        questPingArrow.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerPingArrow.gameObject.activeInHierarchy)
        {
            SetPlayerPingArrow();
        }

        if (IsPointerOverUIElement(out GameObject uiElement))
        {
            

            if (uiElement.tag == "UIElement")
            {
                SelectUIElement(uiElement);
                return;
            }
            else if(uiElement.tag == "PlayerPing")
            {
                SelectUIElement(uiElement);
                if (Input.GetMouseButtonDown(0))
                {
                    playerPing.gameObject.SetActive(false);
                    playerPingArrow.gameObject.SetActive(false);
                }
                return;
            }

            Vector2 mousePos = Input.mousePosition;

            if (IsPointInMap(mousePos, Top.position, Bottom.position, Left.position, Right.position) && Input.GetMouseButtonDown(0))
            {
                playerPing.position = mousePos;
                playerPing.gameObject.SetActive(true);
                playerPingArrow.gameObject.SetActive(true);
            }
        }
        
        if(overedObject != null)
        {
            overedObject.GetComponentInParent<MapUIElement>().SetTextActive(false);
            overedObject = null;
        }
    }

    private void SetPlayerPingArrow()
    {
        Vector3 newPos = MapUI.instance.GetWorldPosFromUIPos(playerPing.position);
        Vector2 dir = newPos - playerT.position;
        playerPingArrow.right = dir.normalized;

        if(doesArrowsFollowPlayer)
        {
            playerPingArrow.transform.position = Camera.main.WorldToScreenPoint(playerT.position);
        }

        playerPingArrow.GetChild(0).gameObject.SetActive(dir.magnitude > distanceToDesappear);
        
    }

    private void SetQuestPingArrow()
    {

    }

    public void SelectUIElement(GameObject uiElement)
    {
        if (uiElement != overedObject)
        {
            if (overedObject != null) overedObject.GetComponentInParent<MapUIElement>().SetTextActive(false);
            overedObject = uiElement;
            overedObject.GetComponentInParent<MapUIElement>().SetTextActive(true);
        }
    }

    public bool IsPointInMap(Vector2 point, Vector2 top, Vector2 bot, Vector2 left, Vector2 right)
    {
        return IsPointInTriangle(point, left, top, right) ||
               IsPointInTriangle(point, left, bot, right);
    }

    // Is the Point in the triangle
    bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
    {
        float d1 = Sign(point, a, b);
        float d2 = Sign(point, b, c);
        float d3 = Sign(point, c, a);

        bool has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }

    // Get the Sign of a triangle
    float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }
}
