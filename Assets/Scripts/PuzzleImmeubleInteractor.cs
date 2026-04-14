using UnityEngine;

public class PuzzleImmeubleInteractor : MonoBehaviour, IInteractable
{
    [Header("State")]
    public bool PuzzleSolved;
    private bool playerInRange;

    [Header("UI")]
    private GameObject interactCanvas;

    [Header("Puzzle Data")]
    [SerializeField] private DialogueData puzzleData;
    [SerializeField] private GameObject displayToShow;
    [SerializeField] private string puzzleID;

    private bool triggered;
    private DialogueData currentData;

    private void Start()
    {
        interactCanvas = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        GameEvents.OnPuzzle2Completed += HandlePuzzleCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnPuzzle2Completed -= HandlePuzzleCompleted;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!PuzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = true;
            interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!PuzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = false;
            interactCanvas.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!playerInRange || triggered) return;

        StartPuzzleLogic();
    }

    // =========================
    // START PUZZLE
    // =========================
    private void StartPuzzleLogic()
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

        interactCanvas.SetActive(false);

        triggered = true;
    }

    // =========================
    // END PUZZLE (EVENT)
    // =========================
    private void HandlePuzzleCompleted(string id)
    {
        if (id != puzzleID) return;

        EndPuzzleLogic();
    }

    public void EndPuzzleLogic()
    {
        if (currentData?.indiceData != null)
        {
            NoteSaveManager.SaveNote(currentData.indiceData);
            Debug.Log("Indice saved: " + currentData.indiceData.name);
        }

        currentData = null;

        //InputManager.Instance.EnablePlayerMovement();
        PuzzleSolved = true;

        if (DialogueManager.Instance == null) return;
        if (DialogueManager.Instance.IsDialogueActive) return;
        DialogueManager.Instance.StartDialogue(puzzleData);
    }

    public void ShowKeypad()
    {
        // NOT USED
    }
}