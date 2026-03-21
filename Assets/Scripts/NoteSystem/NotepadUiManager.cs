using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Transform onSelectNotePopup;

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
        List<string> currentPageDescriptions = new(NOTES_PER_PAGE);

        for (int i = 0;i < NOTES_PER_PAGE; i++)
        {
            notePadTexts[i].text = "";
            var currentIndex = i + (page * NOTES_PER_PAGE) - NOTES_PER_PAGE;
            if(currentIndex < notes.Count)
            {
                currentPageDescriptions.Add(notes[currentIndex]?.Description);
                notePadTexts[i].text = currentPageDescriptions[i];
            }
        }
    }

    /// <summary>
    /// Set the content for the popup note when hovering a note's description on the notepad
    /// </summary>
    /// <param name="index">On the hover, the index is set as the position in the list of notepad texts 0 being the first notepadText transform</param>    
    public void SetNotePopupUiContent(int index)
    {
        // Check how many pages there is according to how many notes were saved.
        var currentIndex = index + (currentPage * NOTES_PER_PAGE) - NOTES_PER_PAGE;
        var noteDescription = onSelectNotePopup.GetChild(1).GetComponent<TextMeshProUGUI>();
        var noteImage = onSelectNotePopup.GetChild(2).GetComponent<Image>();
        var noteExtraInfo = onSelectNotePopup.GetChild(3).GetComponent<TextMeshProUGUI>();

        if (currentIndex < notes.Count)
        {
            noteDescription.text = notes[currentIndex]?.Description;
            noteImage.sprite = (Sprite) AssetDatabase.LoadAssetAtPath(notes[currentIndex]?.SpritePath, typeof(Sprite));
            noteExtraInfo.text = notes[currentIndex]?.ExtraInformation;
        }
    }
}
