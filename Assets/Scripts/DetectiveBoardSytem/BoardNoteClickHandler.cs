using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handler when you click on an image of the board note. Remove that image of the board and its stored collection
/// </summary>
public class BoardNoteClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private DetectiveBoardNoteHandler detectiveBoardNoteHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Sibling index 0 = the background of the detective board
        var transformChildIndex = transform.parent.GetSiblingIndex();
        detectiveBoardNoteHandler.RemoveBoardNoteAt(transformChildIndex);
    }
}
