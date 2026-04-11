using UnityEngine;
using UnityEngine.InputSystem;

public class SelfInteraction : DialogueInteraction
{
    [SerializeField] private bool triggered = false;

    void Update()
    {
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DialogueManager.Instance.Next();
        }
    }

    protected override void OnPlayerEnter()
    {
        if (triggered) return;

        triggered = true;
        StartDialogue();
    }
}