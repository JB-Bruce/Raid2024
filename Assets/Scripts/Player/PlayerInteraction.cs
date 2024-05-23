using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;

    public List<Container> containers = new List<Container>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private Container GetNearestContainer()
    {
        if (containers.Count < 1 )
        {
            return null;
        }

        Container nearestContainer = containers[0];
        float nearestContainerDistance = Vector3.Distance(transform.position, containers[0].transform.position);

        for (int i = 1; i < containers.Count; i++)
        {
            if (containers[i] != null)
            {
                float distance = Vector3.Distance(transform.position, containers[i].transform.position);
                if (distance < nearestContainerDistance)
                {
                    nearestContainer = containers[i];
                    nearestContainerDistance = distance;
                }
            }
        }

        return nearestContainer;
    }

    public void OpenContainer(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Container container = GetNearestContainer();
            if (container != null)
            {
                return;
            }
        }
    }
}
