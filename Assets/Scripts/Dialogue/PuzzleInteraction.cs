using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleInteraction : Interaction
{
    [SerializeField] private bool triggered = false;
    [SerializeField] private GameObject puzzle2;

    protected override void OnPlayerEnter()
    {
        if (triggered) return;

        triggered = true;

        StartDialogue();

        FindAnyObjectByType<NoirMouvement>().GetComponent<PlayerInput>().enabled = false;
        puzzle2.SetActive(true);

    }


}


