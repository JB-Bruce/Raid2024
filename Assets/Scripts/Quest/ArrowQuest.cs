using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrowQuest : MonoBehaviour
{
    public static  ArrowQuest Instance;

    public Transform QuestTarget;

    [SerializeField] private GameObject Arrow;

    private Transform _selfTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        _selfTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!QuestTarget)
        {
            Arrow.SetActive(false);
            return;
        }

        Vector3 direction = QuestTarget.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Vector3.Distance(QuestTarget.position, _selfTransform.position) < 13.0f)
        {
            Arrow.SetActive(false);
        }
        else
        {
            if(!Arrow.activeSelf)
                Arrow.SetActive(true);
        }

    }
}
