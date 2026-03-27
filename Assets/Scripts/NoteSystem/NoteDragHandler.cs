using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Note drag handler script on the text element of a note inside the Notepad ui element.
/// </summary>
public class NoteDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public int NoteId;
    /// <summary>
    /// Temporary game object to appear when the note is dragged
    /// </summary>
    [SerializeField]
    private GameObject noteDragTemp;

    private IndiceData dataToDrag;
    private InputAction mouseAction;

    private void Start()
    {
        mouseAction = InputSystem.actions.FindAction("Mouse");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        noteDragTemp.SetActive(true);
        noteDragTemp.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = mouseAction.ReadValue<Vector2>();
        noteDragTemp.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        noteDragTemp.SetActive(false);
        var raycastTargetGo = eventData.pointerCurrentRaycast.gameObject;
        DetectiveBoardNoteHandler handler;
        // Check if mouse over detective board. Make sure NoteTmp can't be the raycast target in the inspector
        // handler component is on the background of the detective board in the DetectiveBoard canvas
        if (raycastTargetGo.TryGetComponent(out handler))
        {
            var note = NoteSaveManager.GetNoteById(NoteId);
            handler.SetNextBoardNote(note);
        }
    }
}
