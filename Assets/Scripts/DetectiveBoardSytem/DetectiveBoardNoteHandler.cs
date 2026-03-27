using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script component on the background of the detective board to get the content of the notes on the board
/// </summary>
public class DetectiveBoardNoteHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> boardNotes;

    /// <summary>
    /// Set the next available board note that is not filled
    /// </summary>
    /// <param name="note"></param>
    public void SetNextBoardNote(Note note)
    {
        var boardNote = boardNotes.First(go => !go.activeInHierarchy);
        SetBoardNoteContentFor(boardNote, note);
    }

    /// <summary>
    /// Set the content of one board note on the Detective Board
    /// </summary>
    /// <param name="index">Index of children inside parent transform (DetectiveBoard)</param>
    /// <param name="note">Note reference passed to children to set the board note data</param>
    private void SetBoardNoteContentFor(GameObject boardNote, Note note)
    {
        if (boardNote == null) return;
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
