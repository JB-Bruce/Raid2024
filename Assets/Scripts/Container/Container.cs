using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] 
    private GameObject _containerSelectedSprite;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _containerSelectedSprite.SetActive(true);
            PlayerInteraction.Instance.containers.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _containerSelectedSprite.SetActive(false);
            PlayerInteraction.Instance.containers.Remove(this);
        }
    }
}
