using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class NoirMouvement : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 movement;

    [SerializeField] private Animator anim;
    

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

        if (movement.magnitude != 0)
        {
            anim.SetFloat("Movement", speed, 0.1f, Time.deltaTime);
        }
        else 
        {
            anim.SetFloat("Movement", 0);
        }

    }

    void Update()
    {
        transform.Translate(Vector2.right * movement.x * speed * Time.deltaTime);

    }
}
