using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer _checkpointRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _checkpointRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TutorialManager.Instance.NextTutorial();
            this.gameObject.SetActive(false);
        }
    }
}
