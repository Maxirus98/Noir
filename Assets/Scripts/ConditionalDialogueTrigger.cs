using System.Collections.Generic;
using UnityEngine;

public class ConditionalDialogueTrigger : MonoBehaviour
{
    [Header("IndiceData ID requis")]
    [SerializeField] private List<int> requiredIndiceIds = new List<int>();

    [Header("DialogueData Flag requis")]
    [SerializeField] private string requiredFlag;

    [Header("Dialogue to trigger")]
    [SerializeField] private DialogueData dialogueToTrigger;

    [Header("One time event ID")]
    [SerializeField] private string alreadyTriggeredFlag;


    private void Start()
    {
    }

    private void OnEnable()
    {
        GameEvents.On3VolsFound += CheckConditions;
    }

    private void OnDisable()
    {
        GameEvents.On3VolsFound -= CheckConditions;
    }

    public void CheckConditions()
    {
        // Déjà trigger ?
        if (ProgressionManager.Instance.HasFlag(alreadyTriggeredFlag))
            return;
        //// Vérifier flag requis
        //if (!ProgressionManager.Instance.HasFlag(requiredFlag))
        //    return;

        // Vérifier indices
        var notes = NoteSaveManager.GetSavedNotes().notes;

        foreach (int id in requiredIndiceIds)
        {
            if (!notes.Exists(n => n.Id == id))
                return;
        }

        print("2");

        // Marquer comme déjà trigger
        ProgressionManager.Instance.SetFlag(alreadyTriggeredFlag);

        // Trigger dialogue
        DialogueManager.Instance.StartDialogue(dialogueToTrigger);
    }
}