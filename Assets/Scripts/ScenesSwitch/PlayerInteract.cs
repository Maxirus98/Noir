using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentInteractable;

    void Update()
    {
        if (currentInteractable != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact();
        }

        if (currentInteractable != null && Keyboard.current.eKey.wasPressedThisFrame && CompareTag("Keypad")) 
        {
            currentInteractable.ShowKeypad();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
        }
            
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable = null;
        }
           
    }
}
