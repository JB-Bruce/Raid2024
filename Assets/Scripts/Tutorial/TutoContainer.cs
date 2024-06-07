using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutoContainer : MonoBehaviour
{
    private bool _isInTrigger = false;

    [SerializeField] private GameObject Highlight;

    private void Update()
    {
        if (_isInTrigger && TutorialManager.Instance.TutorialIncrement() == 1
           && Inventory.Instance.isInventoryOpen && Inventory.Instance.currentContainer != null)
        {
            Highlight.SetActive(false);
            TutorialManager.Instance.NextTutorial();
        }
    }

    /// <summary>
    ///     Check if the player enter the trigger zone
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInTrigger = true;       
        }
    }

    /// <summary>
    /// Check if the player leave the trigger zone
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInTrigger = false;
        }
    }
}
