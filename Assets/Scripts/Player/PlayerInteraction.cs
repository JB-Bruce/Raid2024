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

    /// <summary>
    /// Caculates and returns the closest container to the player (if there are any)
    /// </summary>
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

    /// <summary>
    /// Try to open a container, if there are multiple, opens the closest
    /// </summary>
    public void OpenContainer(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Container container = GetNearestContainer();
            if (container != null)//If a container is close, open the inventory and the container
            {
                Inventory.Instance.OpenInventory();
                Inventory.Instance.currentContainer = container;
                container.OpenContainer();
            }
        }
    }
}
