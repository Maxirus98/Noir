using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();

    public DialogueUI dialogueUI;

    void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(DialogueData dialogue)
    {
        lines.Clear();

        foreach (var line in dialogue.lines)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            dialogueUI.Hide();
            return;
        }

        DialogueLine line = lines.Dequeue();
        dialogueUI.ShowLine(line);
    }

    public void Next()
    {
        if (dialogueUI.IsTyping)
        {
            dialogueUI.SkipTyping();
        }
        else
        {
            DisplayNextLine();
        }
    }


}