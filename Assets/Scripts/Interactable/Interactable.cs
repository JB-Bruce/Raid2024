using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool _canInterract;

    private void Start()
    {
        _canInterract = true;
    }

    public virtual bool TryToInteract() 
    { 
        if( _canInterract)
        {
            Interact();
        }
        return _canInterract;
    }

    protected virtual void Interact(){}

    public virtual void Highlight(bool state){}
}