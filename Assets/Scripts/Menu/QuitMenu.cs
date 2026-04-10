using UnityEngine;

public class QuitMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject currentCanva;

    public void Quit()
    {
        currentCanva.SetActive(false);
    }

}
