using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class NoirMouvement : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 movement;

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();

        if (movement.x < 0) // A pressed
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        if (movement.x > 0) // D pressed
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    void Update()
    {
        transform.Translate(Vector2.right * movement.x * speed * Time.deltaTime);

    }
}
