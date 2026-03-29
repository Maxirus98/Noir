using UnityEngine;
using UnityEngine.InputSystem;

public class KeypadActive : MonoBehaviour
{
    public GameObject keypad; // Keypad
    public GameObject pressE; // popup text

    private bool playerInRange = false;
    private bool puzzleSolved = false;
    private BoxCollider2D triggerCollider;



    void Awake()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    public void ShowKeypad()
    {
        if (playerInRange && keypad != null)
            keypad.SetActive(true);
    }


    private void OnInteract(InputAction.CallbackContext context)
    {

        if (!puzzleSolved && playerInRange && keypad != null)
        {
            keypad.SetActive(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleSolved && other.CompareTag("Player"))
            playerInRange = true;
        pressE.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!puzzleSolved && other.CompareTag("Player"))
            playerInRange = false;
        pressE.SetActive(false);
    }

    public void PuzzelSolved()
    {
        puzzleSolved = true;
        keypad.SetActive(false); // hide keypad
        playerInRange = false;
        triggerCollider.enabled = false; //Disable interact

    }
}
