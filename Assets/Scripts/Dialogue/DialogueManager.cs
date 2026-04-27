using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();

    public DialogueUI dialogueUI;

    private DialogueData currentDialogue;


    float nextInputTime = 0f;
    [SerializeField] float inputDelay = 0.1f;

    public bool IsDialogueActive { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        // Disable input noir 
        InputManager.Instance.DisablePlayerMovement();

        // Désactiver le notepad
        GameEvents.OnDialogueStart?.Invoke(true);
        // Vérifier les conditions
        if (dialogue.requiredFlags != null)
        {
            //foreach (string flag in dialogue.requiredFlags)
            //{
            //    if (!ProgressionManager.Instance.HasFlag(flag))
            //    {
            //        Debug.Log("Dialogue locked. Missing flag: " + flag);
            //        return;
            //    }
            //}
            if (!ProgressionManager.Instance.HasAllFlags(dialogue.requiredFlags))
                return;
        }

        currentDialogue = dialogue; // set indice data

        lines.Clear();
        IsDialogueActive = true;

        foreach (DialogueLine line in dialogue.lines)
        {
            lines.Enqueue(line);
        }

        if (dialogueUI != null)
            dialogueUI.gameObject.SetActive(true);

        DisplayNextLine();

        // Flags
        if (dialogue.setFlags != null)
        {
            foreach (string flag in dialogue.setFlags)
            {
                ProgressionManager.Instance.SetFlag(flag);
            }
        }
    }



    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            // reanable input move noir
            return;
        }

        DialogueLine line = lines.Dequeue();
        dialogueUI.ShowLine(line);
    }

    public void Next()
    {
        if (!IsDialogueActive)
            return;

        if (Time.time < nextInputTime)
            return;
        nextInputTime = Time.time + inputDelay;

        if (dialogueUI.IsTyping)
        {
            dialogueUI.SkipTyping();
        }
        else
        {
            DisplayNextLine();
        }
    }

    void EndDialogue()
    {
        IsDialogueActive = false;

        if (dialogueUI != null)
            dialogueUI.Hide();

        // sauvegarde l'indice
        if (currentDialogue != null && currentDialogue.indiceData != null )
        {
            NoteSaveManager.SaveNote(currentDialogue.indiceData);
            Debug.Log("Indice saved: " + currentDialogue.indiceData.name);
        }

       // CHECK MID-VILLAIN(UNE SEULE FOIS)
        if (ProgressionManager.Instance.HasFlag("Meet_Villain") &&
        !ProgressionManager.Instance.HasFlag("Villain_DONE"))
        {
            Debug.Log("VillainDone");

            // marquer comme déjà fait
            ProgressionManager.Instance.SetFlag("Villain_DONE");

            // trigger event
            GameEvents.OnMidVillainDone?.Invoke();
        }

        currentDialogue = null;

        // Réactiver input
        InputManager.Instance.EnablePlayerMovement();

        // Réactiver le notepad
        GameEvents.OnDialogueStart?.Invoke(false);
    }

    public void ResetDialogue()
    {
        lines.Clear();
        IsDialogueActive = false;

        if (dialogueUI != null)
            dialogueUI.Hide();
    }


}