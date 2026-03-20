using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script on the notepad that shows and populate the different texts inside the notepad. It also manages up to 5 pages for a total of 50 notes.
/// </summary>
public class NotepadUiManager : MonoBehaviour
{
    private const int NOTES_PER_PAGE = 12;
    private const int PAGES_COUNT_TOSHOW_BTNS = 2;
    private int currentPage = 1;
    private int pageCount;

    [SerializeField]
    private Transform notePadTextsContainer;
    private TextMeshProUGUI[] notePadTexts;

    [SerializeField]
    private Transform pageBtnsContainer;
    private List<Note> notes;
    
    private void OnEnable()
    {
        // Populate the note list
        notes = NoteSaveManager.GetSavedNotes().notes;

        // Check how many pages there is according to how many notes were saved.
        pageCount = Mathf.CeilToInt(notes.Count * 1f / NOTES_PER_PAGE);

        // Add page buttons if there is 2 pages or more
        if (pageCount >= PAGES_COUNT_TOSHOW_BTNS)
        {
            for (int i = 0; i < pageCount; i++)
            {
                pageBtnsContainer.GetChild(i).gameObject.SetActive(true);
            }
        }

        // Get all texts
        notePadTexts = notePadTextsContainer.GetComponentsInChildren<TextMeshProUGUI>();

        // On enable, set the content to the 1st page (first 10 notes)
        SetContentForPage(1);
    }


    /// <summary>
    /// Method to generate the current content of a page in the notepad.
    /// </summary>
    /// <param name="page"></param>
    public void SetContentForPage(int page)
    {
        currentPage = page;
        Debug.Log($"Set content for page {page}");
        // TODO: Refactor. Takes the smallest array between the 2 to prevent out of bound exception
        // notePadTexts.Length is ALWAYS 12 (12 children texts)
        var smallestArraySize = notePadTexts.Length <= notes.Count ? notePadTexts.Length : notes.Count;

        for(int i = 0;i < smallestArraySize; i++)
        {
            // Variable to handle pages and what to render in the text box according to the current selected page
            // var nextPageIncrement = currentPage > 1 ? currentPage + (currentPage * NOTES_PER_PAGE) : currentPage;
            int startIndexForPage = i + (page * NOTES_PER_PAGE) - NOTES_PER_PAGE;
            if (notes.Count > 0 && notes[startIndexForPage] != null)
            {
                notePadTexts[i].text = notes[startIndexForPage]?.Description;
            }
        }
    }
}
