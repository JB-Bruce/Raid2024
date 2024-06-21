using UnityEngine;

public class BuildingsTriggers : MonoBehaviour
{
    [SerializeField]
    private Interactable _pnj;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _pnj.Highlight(true);
            _pnj.TriggerEnter(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _pnj.Highlight(false);
            _pnj.TriggerEnter(false);
        }
    }

    // Set PNJ
    public void SetPNJ(Interactable interact)
    {
        _pnj = interact;
    }
}
