using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutoContainer : MonoBehaviour
{
    private bool _isInTrigger = false;

    private void Update()
    {
        if (_isInTrigger && TutorialManager.Instance.TutorialIncrement() == 1
           && Inventory.Instance.isInventoryOpen && Inventory.Instance.currentContainer != null)
        {
            TutorialManager.Instance.NextTutorial();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInTrigger = true;       
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInTrigger = false;
        }
    }
}
