using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject door;
    public GameObject exitTrigger;

    public void OpenDoor()
    {
        door.SetActive(false);
        exitTrigger.SetActive(true);
    }
}
