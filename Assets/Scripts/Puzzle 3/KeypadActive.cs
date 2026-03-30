using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeypadActive : MonoBehaviour, IInteractable

{
    [Header ("GameObject")]
    public GameObject keypad; // Keypad
    public GameObject pressE; // popup text

    private bool playerInRange = false;
    private bool puzzleSolved = false;
    private BoxCollider2D bc;

    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
    }

  

    public void ShowKeypad()
    {
        if (playerInRange && keypad != null)
       {
           keypad.SetActive(true);
       }
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = true;
            pressE.SetActive(true);
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!puzzleSolved && other.CompareTag("Player"))
        {
            playerInRange = false;
            pressE.SetActive(false);
        }
          
    }

    public void PuzzelSolved()
    {
        puzzleSolved = true; //resolution du puzzle
        keypad.SetActive(false); // hide keypad
        playerInRange = false; 
        bc.enabled = false; //Disable interact 
    }

    void IInteractable.Interact()
    {
        Interact();
    }

    private void Interact()
    {
        keypad.SetActive(true);
    }
}