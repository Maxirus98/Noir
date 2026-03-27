using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script component on the background game object of the detective board to get the content of the notes on the board
/// </summary>
public class DetectiveBoardNoteHandler : MonoBehaviour
{
    private const int NOTES_COUNT_NEEDED_TO_SOLVE = 5;
    [Tooltip("Board notes gameobjects from the DetectiveBoard")]
    [SerializeField]
    private List<GameObject> boardNotes;

    /// <summary>
    /// Check the notes that were added to avoid duplicate
    /// </summary>
    private List<Note> notesAdded = new();

    /// <summary>
    /// Set the next available board note that is not filled
    /// </summary>
    /// <param name="note"></param>
    public void SetNextBoardNote(Note note)
    {
        // avoid duplicate entry of a note by Id
        if (notesAdded.Exists(n => n.Id == note.Id))
        {
            // TODO: Sound and GUI Feedback if exists
            return;
        }
        
        var boardNote = boardNotes.FirstOrDefault(go => !go.activeInHierarchy);

        if (boardNote == null) return;

        notesAdded.Add(note);
        SetBoardNoteContentFor(boardNote, note);
        CheckToSolve();
    }

    private void CheckToSolve()
    {
        if(notesAdded.Count >= NOTES_COUNT_NEEDED_TO_SOLVE)
        {
            // Show the wires

        }
    }

    /// <summary>
    /// Set the content of one board note on the Detective Board
    /// </summary>
    /// <param name="index">Index of children inside parent transform (DetectiveBoard)</param>
    /// <param name="note">Note reference passed to children to set the board note data</param>
    private void SetBoardNoteContentFor(GameObject boardNote, Note note)
    {
        //Activate it
        boardNote.SetActive(true);

        // Set Board Note Description
        boardNote.GetComponentInChildren<TextMeshProUGUI>(true).text = note.Description;
        // Set Board Note Image
        var boardNoteImages = boardNote.GetComponentsInChildren<Image>(true);
        var boardNoteImage = boardNoteImages.FirstOrDefault(go => go.name.Equals("NoteImage"));
        boardNoteImage.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(note.SpritePath, typeof(Sprite));
    }
}
