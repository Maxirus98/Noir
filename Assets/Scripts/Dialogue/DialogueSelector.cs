using System.Collections.Generic;
using UnityEngine;

public class DialogueSelector : MonoBehaviour
{
    public List<DialogueData> dialogues;

    public DialogueData GetValidDialogue()
    {
        DialogueData chosenDialogue = null;

        foreach (DialogueData dialogue in dialogues)
        {
            bool valid = true;

            if (dialogue.requiredFlags != null)
            {
                foreach (string flag in dialogue.requiredFlags)
                {
                    if (!ProgressionManager.Instance.HasFlag(flag))
                    {
                        valid = false;
                        break;
                    }
                }
            }

            if (valid)
                chosenDialogue = dialogue; // garde le dernier valide
        }

        return chosenDialogue;
    }
}