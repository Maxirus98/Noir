using UnityEngine;

public class KeypadActive : MonoBehaviour, IInteractable

{
    public GameObject keypadPuzzleCanvas;
    public GameObject interactCanvas;

    private bool playerInRange = false;
    private bool puzzleSolved = false;

    public void ShowKeypad()
    {
        if (playerInRange && keypadPuzzleCanvas != null)
       {
           keypadPuzzleCanvas.SetActive(true);
       }
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = true;
            interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!puzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = false;
            interactCanvas.SetActive(false);
        }
    }

    public void PuzzelSolved()
    {
        puzzleSolved = true; //resolution du puzzle
        keypadPuzzleCanvas.SetActive(false); // hide keypad
        interactCanvas.SetActive(false);
        playerInRange = false;
    }

    void IInteractable.Interact()
    {
        Interact();
    }

    private void Interact()
    {
        keypadPuzzleCanvas.SetActive(true);
    }
}