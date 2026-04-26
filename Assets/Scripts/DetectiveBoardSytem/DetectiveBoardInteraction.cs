using UnityEngine;

public class DetectiveBoardInteraction : MonoBehaviour, IInteractable
{
    private NotepadUiManager notepadUi;
    [SerializeField]
    private GameObject detectiveBoardUi;

    public void Interact()
    {
        Debug.Log("interacted with board");
        detectiveBoardUi.SetActive(!detectiveBoardUi.activeInHierarchy);
        notepadUi = FindAnyObjectByType<NotepadUiManager>(FindObjectsInactive.Include);
        notepadUi.gameObject.SetActive(detectiveBoardUi.activeInHierarchy);
    }

    public void ShowKeypad()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
