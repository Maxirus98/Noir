using UnityEngine;

public class DetectiveBoardInteraction : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject notepadUi;
    [SerializeField]
    private GameObject detectiveBoardUi;

    public void Interact()
    {
        Debug.Log("interacted with board");
        notepadUi.SetActive(!notepadUi.activeInHierarchy);
        detectiveBoardUi.SetActive(!detectiveBoardUi.activeInHierarchy);
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
