using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapUIOver : MonoBehaviour
{
    GameObject overedObject;

    public Transform Top;
    public Transform Bottom;
    public Transform Left;
    public Transform Right;

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

    void Update()
    {
        if (IsPointerOverUIElement(out GameObject uiElement))
        {
            

            if (uiElement.tag == "UIElement")
            {
                if(uiElement != overedObject)
                {
                    if(overedObject != null) overedObject.GetComponentInParent<MapUIElement>().SetTextActive(false);
                    overedObject = uiElement;
                    overedObject.GetComponentInParent<MapUIElement>().SetTextActive(true);
                    
                }
                return;
            }
            else if (uiElement.tag == "MapBackground")
            {
                if (Input.GetMouseButtonDown(0))
                {

                print("tttyyy");
                }
            }
        }
        
        if(overedObject != null)
        {
            overedObject.GetComponentInParent<MapUIElement>().SetTextActive(false);
            overedObject = null;
        }
    }
}
