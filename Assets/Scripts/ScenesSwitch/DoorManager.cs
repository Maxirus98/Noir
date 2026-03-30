using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


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
}

