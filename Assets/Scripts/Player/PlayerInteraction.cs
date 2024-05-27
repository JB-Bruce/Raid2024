using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;

    private Inventory _inventory;

    public List<Container> containers = new List<Container>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _inventory = Inventory.Instance;
    }

    /// <summary>
    /// Select the nearest container
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < containers.Count; i++)
        {
            if (containers[i] != null)
            {
                containers[i].containerSelectedSprite.SetActive(false);
            }
        }
        Container container = GetNearestContainer();
        if (container != null)
        {
            container.containerSelectedSprite.SetActive(true);
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
    public void PlayerInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_inventory.isInventoryOpen) 
            {
                _inventory.OpenInventory();
                return;
            }
            Container container = GetNearestContainer();
            //PNJ pnj = GetNearestPNJ();
            //if (pnj != null && container != null)
            //if (Vector3.Distance(transform.position, pnj.transform.position) < Vector3.Distance(transform.position, container.transform.position))
            //Open the pnj
            //else
            if (container != null)//If a container is close, open the inventory and the container
            {
                _inventory.OpenInventory();
                _inventory.currentContainer = container;
                container.OpenContainer();
            }
        }
    }
}
