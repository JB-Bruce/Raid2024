using UnityEngine;

public class MapElement : MonoBehaviour
{
    public Sprite sp;
    public Color color;
    public float size;
    public bool canMove;
    public string overName;

    private void Start()
    {
        if (!canMove)
            MapUI.Instance.SetElementToMap(transform.position, sp, color, size, overName);
        else
            MapUI.Instance.SetMovingElementToMap(transform, sp, color, size, overName);
    }
}
