using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();

    public DialogueUI dialogueUI;


    float nextInputTime = 0f;
    [SerializeField] float inputDelay = 0.1f;

    public bool IsDialogueActive { get; set; }

    void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(DialogueData dialogue)
    {
        // Disable input noir 
        FindAnyObjectByType<NoirMouvement>().GetComponent<PlayerInput>().enabled = false;

        // Vérifier les conditions de progression
        if (dialogue.requiredFlags != null)
        {
            foreach (string flag in dialogue.requiredFlags)
            {
                if (!ProgressionManager.Instance.HasFlag(flag))
                {
                    Debug.Log("Dialogue locked. Missing flag: " + flag);
                    return;
                }
            }
        }

        lines.Clear();
        IsDialogueActive = true;

        foreach (DialogueLine line in dialogue.lines)
        {
            lines.Enqueue(line);
        }

        if (dialogueUI != null)
            dialogueUI.gameObject.SetActive(true);

        DisplayNextLine();

        // Appliquer les flags de progression
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

        // todo: faire meilleur avec input manager 
        FindAnyObjectByType<NoirMouvement>().GetComponent<PlayerInput>().enabled = true;
    }

    public void ResetDialogue()
    {
        lines.Clear();
        IsDialogueActive = false;

        if (dialogueUI != null)
            dialogueUI.Hide();
    }
}