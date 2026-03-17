using UnityEngine;
using UnityEngine.InputSystem;

public class LoupeInteractionHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject clueFound;
    private GameObject clueFoundInstance;

    private GameObject loupeSliderInstance;
    private InputAction loupeInspectAction;
    private InputAction mouseAction;
    private float timeInspecting;
    private bool finishInspecting;

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loupeInspectAction = InputSystem.actions.FindAction("Inspect");
        mouseAction = InputSystem.actions.FindAction("Mouse");
        loupeSliderInstance = transform.Find("LoupeSlider").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = mouseAction.ReadValue<Vector2>();
        loupeSliderInstance.SetActive(loupeInspectAction.IsPressed());
        // TODO: Play sound when pressed

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        if (!loupeSliderInstance.activeInHierarchy)
        {
            // Reset inspecting data
            timeInspecting = 0f;
            finishInspecting = false;
        }
        else
        {
            InspectLoupeArea();
        }
    }

    /// <summary>
    /// Set visual effect for the LoupeArea when pressed
    /// </summary>
    private void InspectLoupeArea()
    {
        timeInspecting += Time.deltaTime;
        timeInspecting = Mathf.Min(timeInspecting, 1f);
        finishInspecting = timeInspecting >= 1f;
        var sliderSR = loupeSliderInstance.GetComponent<SpriteRenderer>();

        // Manage custom shader circular slider
        sliderSR.material.SetFloat("_Frac", timeInspecting);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var isOverIndice = collision.gameObject.CompareTag("Indice");
        if (isOverIndice && finishInspecting)
        {
            var indice = collision.GetComponent<Indice>();
            OnFinishInspecting(collision.transform.position, indice);
        }
    }

    /// <summary>
    /// Audio visual effect that triggers when the inspection is done and add the indice found to the note system.
    /// </summary>
    private void OnFinishInspecting(Vector3 cluePosition, Indice indice)
    {
        if(clueFoundInstance == null)
        {
            clueFoundInstance = Instantiate(clueFound, cluePosition, Quaternion.identity);
            var handler = clueFoundInstance.GetComponent<ClueFoundHandler>();

            // Change clue found scale according to the indice component
            handler.transform.localScale = Vector3.one * indice.clueFoundScale;

            // Pass the indice data info to the clue found instance
            handler.indiceData = indice.data;
        }
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}
