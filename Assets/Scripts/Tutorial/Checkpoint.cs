using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Checkpoint : MonoBehaviour
{
  
    /// <summary>
    /// Check if the collision is the player if so deactivate the checkpoint and call the function to go to the next step int the tutorial
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TutorialManager.Instance.NextTutorial();
            this.gameObject.SetActive(false);
        }
    }
    
}
