using UnityEngine;
using UnityEngine.UIElements;

public class AimLaser : MonoBehaviour
{

    [SerializeField] private float DistanceRay = 100;

    public Transform laserFirePoint;
    public LineRenderer lineRenderer;
    new Transform transform;
    
    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    //If the raycast touch a box collider, call Draw2DRayCollision. Else, call Draw2DRay
    public void ShootLaser(Vector3 direction)
    {

        if (Physics2D.Raycast(laserFirePoint.position, direction))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, direction);
            Draw2DRayToCollision(laserFirePoint.position, _hit.point);
        }
        else
        {

            Draw2DRay(laserFirePoint.position, direction * DistanceRay);
        }

    }

    //Define a beggining point for the laser , and a ending point base on the player position
    private void Draw2DRay(Vector2 startPos, Vector2 dir)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos + dir);
    }

    //Define a beggining point for the laser , and a ending point 
    private void Draw2DRayToCollision(Vector2 startPos, Vector2 endpos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endpos);
    }

    private void Update()
    {
        //ShootLaser();
    }
}
