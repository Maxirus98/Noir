using UnityEngine;

/// <summary>
/// Script that lets the player see an interact pop up if the puzzle is not yet solved.
/// </summary>
public class PuzzleImmeubleInteractor : MonoBehaviour, IInteractable
{
    public bool PuzzleSolved;
    private bool playerInRange;

    /// <summary>
    /// Interact canvas is the first child of this object
    /// </summary>
    private GameObject interactCanvas;

    private void Start()
    {
        interactCanvas = transform.GetChild(0).gameObject;
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
        if (playerInRange)
        {
            // TODO: Do something do start the puzzle
            // TODO: Move camera to the top of the building
            // TODO: Let the puzzle 3 start
            Debug.Log("Player has interacted with immeuble");
        }
    }

    public void ShowKeypad()
    {
        // NOT USED
    }
}
