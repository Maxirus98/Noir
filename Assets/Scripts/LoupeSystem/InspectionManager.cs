using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script on the player to spawn Loupe
/// </summary>
public class InspectionManager : MonoBehaviour
{
    [SerializeField] GameObject loupe;
    [SerializeField] GameObject loupeAreaHidden;

    GameObject loupeInstance;
    GameObject loupeAreaHiddenInstance;

    InputAction loupeModeAction;

    // TODO: Disable CharacterController when Loupe is on.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loupeModeAction = InputSystem.actions.FindAction("Loupe");
    }

    // Update is called once per frame
    void Update()
    {
        if(loupeModeAction.WasPerformedThisFrame())
        {
            ToggleLoupe();
        }
    }

    public void ToggleLoupe()
    {
        if (loupeAreaHiddenInstance == null)
        {
            loupeAreaHiddenInstance = Instantiate(loupeAreaHidden);
        }
        else
        {
            Destroy(loupeAreaHiddenInstance);
            loupeAreaHiddenInstance = null;
        }

        if (loupeInstance == null)
        {
            loupeInstance = Instantiate(loupe);
        }
        else 
        {
            Destroy(loupeInstance);
            loupeInstance = null;
        }
    }
}
