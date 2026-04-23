using UnityEngine;

public class DupDestroy : MonoBehaviour
{
    public static DupDestroy instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);  
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    /// <summary>
    /// Method to delete the json files to reset the game. 
    /// This method is called when player quit game
    /// </summary>
    private void OnApplicationQuit()
    {
        NoteSaveManager.DeleteAllNotes();
    }
}
