using UnityEngine;

public class MapElement : MonoBehaviour
{
    public Sprite sp;
    public Color color;
    public float size;
    public bool canMove;

    private void Start()
    {
        if (!canMove)
            MapUI.Instance.SetElementToMap(transform.position, sp, color, size);
        else
            MapUI.Instance.SetMovingElementToMap(transform, sp, color, size);
    }
}
