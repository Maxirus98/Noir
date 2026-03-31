using UnityEditor;
using UnityEngine;

/// <summary>
/// This script is on a gameobject of type Indice
/// </summary>
public class Indice : MonoBehaviour
{
    /// <summary>
    /// The scriptable object created specific for this indice
    /// </summary>
    [field: SerializeField]
    public IndiceData data { get; private set; }

    /// <summary>
    /// The scale to spawn the (!) clueFound sprite.
    /// </summary>
    [field: SerializeField]
    public float clueFoundScale { get; set; } = 1f;

    /// <summary>
    /// Indice is not interactable with if marked as found.
    /// </summary>
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
