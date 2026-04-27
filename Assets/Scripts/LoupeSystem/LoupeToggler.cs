using UnityEngine;
using UnityEngine.UI;

public class LoupeToggler : MonoBehaviour
{
    // On the player gameobject
    private InspectionManager inspectionManager;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        GameEvents.OnToggleDetectiveBoard += DisableLoupeToggle;
        GameEvents.OnDialogueStart += DisableLoupeToggle;
    }

    private void OnDisable()
    {
        GameEvents.OnToggleDetectiveBoard -= DisableLoupeToggle;
        GameEvents.OnDialogueStart -= DisableLoupeToggle;
    }

    private void DisableLoupeToggle(bool disableLoupe) { toggle.interactable = !disableLoupe; }

    void Start()
    {
        toggle.onValueChanged.AddListener(delegate
        {
            ToggleLoupe();
        });
    }

    private void ToggleLoupe()
    {
        inspectionManager = FindAnyObjectByType<InspectionManager>(FindObjectsInactive.Include);
        inspectionManager.ToggleLoupe();
    }
}
