using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool _canInterract = true;

    //call when the player interact with this and return true if he can interact
    public virtual bool TryToInteract() 
    {
        if( _canInterract)
        {
            Interact();
        }
        return _canInterract;
    }

    //call when the player interact with this and this can interact
    protected virtual void Interact(){}

    public virtual void Highlight(bool state){}
    public virtual void TriggerEnter(bool state) {}
}