using System.Collections;
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
    /// <summary>
    /// -1 because the transformSiblingIndex starts at 0 but 0 is the background and
    /// not our corresponding note object which is index 1.
    /// </summary>
    private const int NUMBER_OF_SIBLINGS_BEFORE_NOTES_GO = 1;
    private const float ROPES_ANIMATION_TIME_SECONDS = 3f;

    [Tooltip("Board notes gameobjects from the DetectiveBoard")]
    [SerializeField]
    private List<GameObject> boardNotes;

    [Tooltip("Ropes to activate when there is 5 notes on the board to check if the solution is correct or wrong")]
    [SerializeField]
    private GameObject ropesCheck;

    [Tooltip("AuidoManager singleton to enable playback one shot sounds or music")]
    private AudioManager audioManager;
    private bool canClearBoardNotes = true;
    /// <summary>
    /// Check the notes that were added to avoid duplicates
    /// </summary>
    private List<Note?> notesAdded = new(5);

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnEnable()
    {
        GameEvents.OnToggleDetectiveBoard?.Invoke(true);
    }

    /// <summary>
    /// Remove board note at the corresponding index in the note gameObjects List and 
    /// the List of notes that is currently on the board
    /// </summary>
    /// <param name="transformSiblingIndex"></param>
    public void RemoveBoardNoteAt(int transformSiblingIndex)
    {
        // Prevent removing if solution is complete
        if (notesAdded.Count >= NOTES_COUNT_NEEDED_TO_SOLVE) return;
        // Make the note at the clicked index null in the notesAdded List
        notesAdded[transformSiblingIndex - NUMBER_OF_SIBLINGS_BEFORE_NOTES_GO] = null;
        // Deactivate the board Note at the clicked index
        boardNotes[transformSiblingIndex - NUMBER_OF_SIBLINGS_BEFORE_NOTES_GO].SetActive(false);
    }

    /// <summary>
    /// Clear all the notes. Will be disabled during solution check animation. On a clear all button.
    /// </summary>
    public void ClearBoardNotes()
    {
        // Disable if solution check animation
        if(!canClearBoardNotes) return;

        // Clear notes added array
        notesAdded.Clear();
        ropesCheck.SetActive(false);
        // Set inactive all the notes on the board
        foreach (var boardNote in boardNotes) { 
            boardNote.SetActive(false);
        }
    }

    /// <summary>
    /// Set the next available board note that is not filled
    /// </summary>
    /// <param name="note"></param>
    public void SetNextBoardNote(Note note)
    {
        // avoid duplicate entry of a note by Id
        if (notesAdded.Exists(n => n?.Id == note.Id))
        {
            // TODO: Sound and GUI Feedback if exists
            return;
        }
        
        var boardNote = boardNotes.FirstOrDefault(go => !go.activeInHierarchy);

        if (boardNote == null) return;

        // Replace the first null if found otherwise append it to the end of the list
        var firstNullIndex = notesAdded.FindIndex(n => n == null);
        if (firstNullIndex > -1)
        {
            notesAdded[firstNullIndex] = note;
        }
        else
        {
            notesAdded.Add(note);
        }

        SetBoardNoteContentFor(boardNote, note);
        if(notesAdded.Count >= NOTES_COUNT_NEEDED_TO_SOLVE)
        {
            StartCoroutine(nameof(CheckToSolve));
        }
    }

    /// <summary>
    /// Activate the rope which will check if the animation has ended
    /// </summary>
    private IEnumerator CheckToSolve()
    {
        if(!ropesCheck.activeInHierarchy)
        {
            // Show the wires that will be animated and draw themselves on the board
            ropesCheck.SetActive(true);

            // disable board note clear
            canClearBoardNotes = false;
        }

        yield return new WaitForSeconds(ROPES_ANIMATION_TIME_SECONDS);

        var solutionIsCorrect = notesAdded.All(n => n?.CanHelpToSolve == true);
        if (solutionIsCorrect)
        {
            // Solution is correct
            Debug.Log("Solution is correct");

            // TODO: Change Sound Id for Success sound
            audioManager.PlaySound("UI_Submit");
            // TODO: Load end game cinematic
        } else
        {
            // Solution is wrong
            // TODO: Change Sound Id for Fail sound
            audioManager.PlaySound("UI_Back");

            // Enable board note clear
            canClearBoardNotes = true;
            ClearBoardNotes();
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
        boardNoteImage.sprite = Resources.Load<Sprite>(note.SpritePath);
    }

    private void OnDisable()
    {
        GameEvents.OnToggleDetectiveBoard?.Invoke(false);
        ClearBoardNotes();
    }
}
