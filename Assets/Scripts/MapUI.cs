using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Transform TopRight;
    public Transform BotLeft;

    public Transform UITopRight;
    public Transform UIBotLeft;

    List<GameObject> list = new();

    List<(Transform, Transform)> movingMapElements = new();

    public GameObject prefab;
    public Transform parent;

    Vector3 ratio1;
    Vector3 ratio2;

    bool isOpen;

    public GameObject mapUI;

    public static MapUI instance;

    private void Awake()
    {
        instance = this;
        ratio1 = BotLeft.transform.position - TopRight.transform.position;
        ratio2 = UIBotLeft.transform.position - UITopRight.transform.position;
    }

    // UI button to open or close
    public void OpenCloseMap()
    {
        mapUI.SetActive(!mapUI.activeInHierarchy);
    }

    // Put fix element on the UI 
    public void SetElementToMap(Vector3 pos, Sprite sp, Color c, float size)
    {
        GameObject go = Instantiate(prefab, parent);

        Vector3 newPos = Vector3.zero;

        newPos = UIBotLeft.position + (pos - BotLeft.position) * ((UITopRight.position - UIBotLeft.position).magnitude / (TopRight.position - BotLeft.position).magnitude);

        go.GetComponent<Image>().sprite = sp;
        go.GetComponent<Image>().color = c;

        go.transform.localScale *= size;

        list.Add(go);

        go.transform.position = newPos;
    }

    // Put moving element on the UI
    public void SetMovingElementToMap(Transform t, Sprite sp, Color c, float size)
    {
        GameObject go = Instantiate(prefab, parent);
        go.GetComponent<Image>().sprite = sp;
        go.GetComponent<Image>().color = c;
        go.transform.localScale *= size;
        movingMapElements.Add((t, go.transform));
    }

    // Update all the moving elements
    private void Update()
    {
        foreach (var t in movingMapElements)
        {
            Vector3 newPos = Vector3.zero;
            Vector3 pos = t.Item1.position;

            newPos = UIBotLeft.position + (pos - BotLeft.position) * ((UITopRight.position - UIBotLeft.position).magnitude / (TopRight.position - BotLeft.position).magnitude);

            t.Item2.position = newPos;
        }
    }
}
