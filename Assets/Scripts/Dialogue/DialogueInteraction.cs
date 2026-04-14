using UnityEngine;

public abstract class DialogueInteraction : Interaction
{
    [Header("Dialogue")]
    [SerializeField] protected DialogueSelector dialogueSelector;

    [Header("Interaction UI")]
    [SerializeField] private GameObject interactionPrompt;

    private DialogueData currentDialogue;

    protected virtual void Awake()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    protected override void OnPlayerEnter()
    {
        if (dialogueSelector == null) return;

        currentDialogue = dialogueSelector.GetValidDialogue();
        if (currentDialogue == null) return;

        // check if flag has been acquired to display prompt ui inetraction
        if (!ProgressionManager.Instance.HasAllFlags(currentDialogue.requiredFlags))
           return;
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    protected override void OnPlayerExit()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    protected void StartDialogue()
    {
        if (DialogueManager.Instance == null) return;
        if (DialogueManager.Instance.IsDialogueActive) return;
        if (dialogueSelector == null) return;

        currentDialogue = dialogueSelector.GetValidDialogue();

        if (currentDialogue != null)
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            DialogueManager.Instance.StartDialogue(currentDialogue);
        }
    }
}