using UnityEngine;
using UnityEngine.InputSystem;

public class EnterLab : MonoBehaviour
{
    public GameObject interactText;   // "Press E"
    public Transform teleportPoint;   // pour le moment juste un changement de x y

    private bool playerInRange = false;
    private Transform player;
    private PlayerInput playerInput;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerInput = player.GetComponent<PlayerInput>();

        if (playerInput != null)
            playerInput.actions["Interact"].performed += OnInteract;
    }

    void OnDisable()
    {
        if (playerInput != null)
            playerInput.actions["Interact"].performed -= OnInteract;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            player.position = teleportPoint.position;
        }
    }

    public void Teleport()
    {
        if (playerInRange)
        {
            player.position = teleportPoint.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactText.SetActive(false);
        }
    }
}
