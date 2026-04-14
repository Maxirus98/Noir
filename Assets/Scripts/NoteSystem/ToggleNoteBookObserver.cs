using UnityEngine;
using UnityEngine.UI;

public class ToggleNoteBookObserver : MonoBehaviour
{
    [SerializeField]
    private GameObject notepadToggle;
    [SerializeField]
    private GameObject notepad;

    private void OnEnable()
    {
        GameEvents.OnInspect += HideAndPreventNotepad;
        GameEvents.OnDialogueStart += HideAndPreventNotepad;
        GameEvents.OnToggleDetectiveBoard += PreventNotepadToggle;
    }

    private void OnDisable()
    {
        GameEvents.OnInspect -= HideAndPreventNotepad;
        GameEvents.OnDialogueStart -= HideAndPreventNotepad;
        GameEvents.OnToggleDetectiveBoard -= PreventNotepadToggle;
    }

    /// <summary>
    /// Event observer method for Loupe, Dialogue and DetectiveBoard 
    /// </summary>
    private void HideAndPreventNotepad(bool hideNotepad)
    {
        notepadToggle.SetActive(!hideNotepad);
        notepad.SetActive(false);
    }

    private void PreventNotepadToggle(bool intertactable)
    {
        notepadToggle.GetComponent<Toggle>().interactable = intertactable;
    }
}
