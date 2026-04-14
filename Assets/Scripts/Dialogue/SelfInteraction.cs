using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SelfInteraction : DialogueInteraction
{
    [SerializeField] private /*static*/ bool triggered = false;

    private void Start()
    {
        //if (NoteSaveManager.GetSavedNotes().notes.Count <= 0)
        //{
        //    triggered = false;
        //}
    }

    void Update()
    {
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DialogueManager.Instance.Next();
        }
    }

    protected override void OnPlayerEnter()
    {
        if (triggered) return;

        triggered = true;
        StartDialogue();
    }

    //private void OnApplicationQuit()
    //{
    //    triggered = false;
    //}

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //// This method is called whenever a new scene is loaded
    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if(scene.name != "BureauTestD")
    //    {
    //        triggered = false;
    //    }
    //}
}