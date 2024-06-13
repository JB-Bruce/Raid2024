using UnityEngine;

public class BuildingsTriggers : MonoBehaviour
{
    [SerializeField]
    private Pnj _pnj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _pnj.Highlight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _pnj.Highlight(false);
        }
    }
}
