using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


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
            transform.localScale = new Vector3(-0.27f, transform.localScale.y, transform.localScale.z);
        }

        if (movement.x > 0) // D pressed
        {
            transform.localScale = new Vector3(0.27f, transform.localScale.y, transform.localScale.z);
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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "BureauTest")
        {
            transform.localScale = new Vector3(0.27f, 0.27f, 1f);
        }
        else if (scene.name == "BarTest")
        {
            transform.localScale = new Vector3(-0.27f, 0.27f, 1f);
        }
    }
}
