using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private DialogueData dialogue;

    private void Start()
    {
        Interact();
    }
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    void Update()
    {
        // todo:debug a enlever 
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DialogueManager.Instance.Next();
        }
    }
}