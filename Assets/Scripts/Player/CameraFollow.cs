using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.4f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] public Transform target;

    private TutorialManager tutorialManager;

    private void Start()
    {
        if (TutorialManager.Instance)
            tutorialManager = TutorialManager.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = SetTargerPosition() + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    /// <summary>
    ///     Set the target position for the follow camera
    /// </summary>
    private Vector3 SetTargerPosition()
    {
        Vector3 newTargetPosition = target.position;

        if ( tutorialManager && tutorialManager.TutorialIncrement() == 4)
        {
            newTargetPosition = tutorialManager.CenterAlliesTransform.position;
        }

        return newTargetPosition;
    }
}
