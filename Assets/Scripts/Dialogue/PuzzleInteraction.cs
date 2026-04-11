using UnityEngine;

public class PuzzleInteraction : Interaction
{
    [Header("Data")]
    [SerializeField] private DialogueData puzzleData;

    [Header("Puzzle")]
    [SerializeField] private GameObject displayToShow;
    [SerializeField] private string puzzleID;

    private bool triggered;
    private DialogueData currentData;

    void OnEnable()
    {
        GameEvents.OnPuzzle2Completed += HandlePuzzleCompleted;
    }

    void OnDisable()
    {
        GameEvents.OnPuzzle2Completed -= HandlePuzzleCompleted;
    }

    void HandlePuzzleCompleted(string id)
    {
        if (id != puzzleID) return;
        EndPuzzleLogic();
    }

    protected override void OnPlayerEnter()
    {
        if (triggered) return;
        StartPuzzleLogic();
    }

    void StartPuzzleLogic()
    {
        if (puzzleData == null)
        {
            Debug.LogWarning("No puzzleData assigned");
            return;
        }

        if (!ProgressionManager.Instance.HasAllFlags(puzzleData.requiredFlags))
            return;

        InputManager.Instance.DisablePlayerMovement();

        currentData = puzzleData;

        foreach (string flag in puzzleData.setFlags)
        {
            ProgressionManager.Instance.SetFlag(flag);
        }

        if (displayToShow != null)
            displayToShow.SetActive(true);

        triggered = true;
    }

    public void EndPuzzleLogic()
    {
        if (currentData?.indiceData != null)
        {
            NoteSaveManager.SaveNote(currentData.indiceData);
            Debug.Log("Indice saved: " + currentData.indiceData.name);
        }

        currentData = null;
        InputManager.Instance.EnablePlayerMovement();
    }
}