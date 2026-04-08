using System;
using UnityEngine;


public class DoorManager : MonoBehaviour, IInteractable
{
   
    [SerializeField] string levelName;
    public Vector3 spawnPosition;
    

    public void Interact()
    {
        sceneloader.instance.LoadScene(levelName, spawnPosition);
    }   

    void IInteractable.ShowKeypad()
    {
        ShowKeypad();
    }

    private void ShowKeypad()
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Interagir canvas apparait
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Interagir canvas apparait
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}

