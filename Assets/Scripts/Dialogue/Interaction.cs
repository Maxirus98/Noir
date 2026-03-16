using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueSelector dialogueSelector;

    [Header("Interaction UI")]
    [SerializeField] private GameObject interactionPrompt;

    protected bool playerInRange;

    private void Awake()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);

        OnPlayerEnter();
    }


    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        OnPlayerExit();
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }

    protected void StartDialogue()
    {
        if (DialogueManager.Instance == null) return;

        if (DialogueManager.Instance.IsDialogueActive) return;

        if (dialogueSelector == null) return;


        DialogueData dialogue = dialogueSelector.GetValidDialogue();

        if (dialogue != null)
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}