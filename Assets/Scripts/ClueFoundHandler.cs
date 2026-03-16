using UnityEngine;

public class ClueFoundHandler : MonoBehaviour
{
    private InspectionManager inspectionManager;
    private float lifetime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inspectionManager = FindAnyObjectByType<InspectionManager>();
        inspectionManager.ToggleLoupe();
        Destroy(gameObject, lifetime);
    }

    private void OnDestroy()
    {
        // TODO: Créer note
    }
}
