using UnityEngine.InputSystem;

public class NPCInteraction : DialogueInteraction
{
    void Update()
    {
        if (!playerInRange) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartDialogue();
        }

        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DialogueManager.Instance.Next();
        }
    }

    protected override void OnPlayerExit()
    {
        base.OnPlayerExit();

        if (DialogueManager.Instance == null) return;

        DialogueManager.Instance.IsDialogueActive = false;
        DialogueManager.Instance.ResetDialogue();
    }
}