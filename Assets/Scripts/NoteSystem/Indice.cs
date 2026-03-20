using UnityEditor;
using UnityEngine;

public class Indice : MonoBehaviour
{
    [field: SerializeField]
    public IndiceData data { get; private set; }
    [field: SerializeField]
    public float clueFoundScale { get; set; }

    [field: SerializeField]

    public bool clueFound { get; set; }

    private void Awake()
    {
        if (!data.IsIdUnique()) { 
            Debug.LogError($"Fix this error in order to start the game: The id of the {data.name} already exists on another {typeof(IndiceData).Name} ScriptableObject.");
            EditorApplication.isPlaying = false;
        }
    }

    private void Start()
    {
        var noteWrapper = NoteSaveManager.GetSavedNotes();
        var notes = noteWrapper.notes;

        clueFound = notes.Exists(n => n.Id == data.Id);
    }
}
