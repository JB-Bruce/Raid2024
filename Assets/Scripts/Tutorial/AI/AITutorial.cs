using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITutorial : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private float _lerptime = 10;

    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
       _transform.position =  Vector3.Lerp(transform.position, Player.position, (1 * Time.deltaTime) / _lerptime);
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            TutorialManager.Instance.NextTutorial();
            _transform.gameObject.SetActive(false);
        }
    }
}
