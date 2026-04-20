using System.Collections;
using UnityEngine;

public class PuzzleImmeubleInteractor : MonoBehaviour, IInteractable
{
    [Header("State")]
    public bool PuzzleSolved;
    private bool playerInRange;
    private bool isPuzzleActive; // Prevent spam interact while puzzle 2 ongoing

    [Header("UI")]
    private GameObject interactCanvas;

    [Header("Puzzle Data")]
    [SerializeField] private DialogueData puzzleData;
    [SerializeField] private GameObject displayToShow;
    [SerializeField] private string puzzleID;
    [SerializeField] private string uniqueFlag; // triggers one time during the whole game

    
    [Header("Camera Puzzle 2")]
    [SerializeField] private Transform cameraTargetTop;
    [SerializeField] private float delayEndPuzzle2 = 1.5f;
    [SerializeField] private float maxMoveXTime = 1.5f;
    [SerializeField] private float maxZoomTime = 1.5f;
    [SerializeField] private float maxMoveYTime = 1.5f;

    // Save initial camera state
    private float initialZoom;
    private Vector3 initialCamPos;

    private CamerraFollow cam;

    private DialogueData currentData;

    private void Start()
    {
        interactCanvas = transform.GetChild(0).gameObject;
        cam = Camera.main.GetComponent<CamerraFollow>();
    }

    private void OnEnable()
    {
        GameEvents.OnPuzzle2Completed += HandlePuzzleCompleted;
        GameEvents.OnPuzzle2Exited += HandlePuzzleExited;
    }

    private void OnDisable()
    {
        GameEvents.OnPuzzle2Completed -= HandlePuzzleCompleted;
        GameEvents.OnPuzzle2Exited -= HandlePuzzleExited;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if flags exist dont trigger
        if (ProgressionManager.Instance.HasFlag(uniqueFlag))
            return;

        if (!PuzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = true;
            interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // if flags exist dont trigger
        if (ProgressionManager.Instance.HasFlag(uniqueFlag))
            return;

        if (!PuzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = true;
            interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if flags exist dont trigger
        if (ProgressionManager.Instance.HasFlag(uniqueFlag))
            return;

        if (!PuzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = false;
            interactCanvas.SetActive(false);
        }
    }

    public void Interact()
    {
        // if flags exist dont trigger
        if (ProgressionManager.Instance.HasFlag(uniqueFlag))
            return;

        // Already solved 
        if (PuzzleSolved) return;

        // Already active
        if (isPuzzleActive) return;

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

        isPuzzleActive = true;

        //SAVE CAMERA STATE
        initialZoom = Camera.main.orthographicSize;
        initialCamPos = Camera.main.transform.position;

        InputManager.Instance.DisablePlayerMovement();

        currentData = puzzleData;

        foreach (string flag in puzzleData.setFlags)
        {
            ProgressionManager.Instance.SetFlag(flag);
        }

        interactCanvas.SetActive(false);

        // Start sequencing camera to puzzle 2
        StartCoroutine(StartPuzzleSequence());
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
        
        // set flag to avoid triggering it again
        ProgressionManager.Instance.SetFlag(uniqueFlag);

        ReturnCameraGameplay();

        //delay avant dialogue
        StartCoroutine(EndPuzzle2WithDialogue());

    }
    private IEnumerator EndPuzzle2WithDialogue()
    {
        yield return new WaitForSeconds(delayEndPuzzle2);

        if (DialogueManager.Instance == null) yield break;
        if (DialogueManager.Instance.IsDialogueActive) yield break;

        DialogueManager.Instance.StartDialogue(puzzleData);

    }

    private void HandlePuzzleExited(string id)
    {
        if (id != puzzleID) return;

        ReturnCameraGameplay();
    }

    private void ReturnCameraGameplay()
    {
        isPuzzleActive = false;

        cam.StopCinematic();

        interactCanvas.SetActive(false);

        Camera camComponent = Camera.main;
        camComponent.orthographicSize = initialZoom;
    }

    private IEnumerator StartPuzzleSequence()
    {
        initialZoom = Camera.main.orthographicSize;
        initialCamPos = Camera.main.transform.position;
        Camera camComponent = Camera.main;

        Vector3 startPos = camComponent.transform.position;

        Vector3 targetPos = new Vector3(
            cameraTargetTop.position.x,
            cameraTargetTop.position.y,
            startPos.z
        );

        // 1. MOVE X FIRST (horizontal align)
        float timerX = 0f;

        while (Mathf.Abs(camComponent.transform.position.x - targetPos.x) > 0.1f)
        {
            timerX += Time.deltaTime;

            Vector3 pos = camComponent.transform.position;

            pos.x = Mathf.Lerp(pos.x, targetPos.x, Time.deltaTime * 3f);
            camComponent.transform.position = pos;

            print("x");

            if (timerX >= maxMoveXTime) break; // failsafe

            yield return null;
        }

        // 2. ZOOM SMOOTH
        float targetZoom = 2.6f;
        float timerZoom = 0f;

        while (Mathf.Abs(camComponent.orthographicSize - targetZoom) > 0.05f)
        {
            timerZoom += Time.deltaTime;

            camComponent.orthographicSize = Mathf.Lerp(
                camComponent.orthographicSize,
                targetZoom,
                Time.deltaTime * 2f
            );

            print("zoom");

            if (timerZoom >= maxZoomTime) break; // failsafe

            yield return null;
        }

        // 3. MOVE UP (cinematic)
        cam.MoveToCinematic(targetPos);

        float timerY = 0f;

        while (Vector3.Distance(camComponent.transform.position, targetPos) > 10.5f)
        {
            timerY += Time.deltaTime;

            print("y");

            if (timerY >= maxMoveYTime) break; // failsafe

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // 4. SHOW PUZZLE
        if (displayToShow != null)
        {
            displayToShow.SetActive(true);

            CanvasGroup cg = displayToShow.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0;
                float t = 0;

                while (t < 1)
                {
                    t += Time.deltaTime * 2f;
                    cg.alpha = t;
                    yield return null;
                }
            }
        }
    }

    public void ShowKeypad()
    {
        // NOT USED
    }
}