using UnityEngine;
using UnityEngine.InputSystem;

public class SelfInteraction : DialogueInteraction
{
    [SerializeField] private string uniqueFlag; // triggers one time during the whole game

    // disable trigger if flag exist in hashset Progression Manager singleton
    private void Start()
    {
        if (ProgressionManager.Instance.HasFlag(uniqueFlag))
        {
            gameObject.SetActive(false);
        }
    }

    // next dialogue space
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
        // if flags exist dont trigger
        if (ProgressionManager.Instance.HasFlag(uniqueFlag))
            return;

        // set flag to avoid triggering it again
        ProgressionManager.Instance.SetFlag(uniqueFlag);

        StartDialogue();
        
        //ici la logique ctu possible 
    }

}