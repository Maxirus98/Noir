using UnityEngine;
using UnityEngine.UI;

public class LoupeToggler : MonoBehaviour
{
    // On the player gameobject
    private InspectionManager inspectionManager;
    private Toggle toggle;

    void Start()
    {
        inspectionManager = FindAnyObjectByType<InspectionManager>();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate
        {
            ToggleLoupe();
        });
    }

    private void ToggleLoupe()
    {
        inspectionManager.ToggleLoupe();
    }
}
