using TMPro;
using UnityEngine;

public class InteractableText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _interactableName;

    [SerializeField]
    private TextMeshProUGUI _errorMessage;

    private void UpdateInteractableName(string name)
    {
        _interactableName.text = name;
        _interactableName.gameObject.SetActive(true);
    }

    private void UpdateErrorMessage(string error)
    {
        _errorMessage.text = error;
        _errorMessage.gameObject.SetActive(true);
    }

    public void ShowText(Interactable interactable)
    {
        string name = "";
        string error = "";
        if (interactable is Pnj pnj)
        {
            name = pnj.GetName;
        }
        else if (interactable is DroppedItem droppedItem)
        {
            name = droppedItem.item.Name;
            if (Inventory.Instance.IsInventoryFull())
            {
                error = "Inventaire Plein";
            }
        }
        else if (interactable is Container container)
        {
            name = container.name;
        }
        else if (interactable is MainQuestInteractable mainQuestInteractable)
        {
            name = mainQuestInteractable.Name;
        }
        else if (interactable is Tank tank)
        {
            name = "Cuve";
        }
        else
        {
            return;
        }
        UpdateInteractableName(name);
        UpdateErrorMessage(error);
    }

    public void HideText()
    {
        _interactableName.gameObject.SetActive(false);
        _errorMessage.gameObject.SetActive(false);
    }
}
