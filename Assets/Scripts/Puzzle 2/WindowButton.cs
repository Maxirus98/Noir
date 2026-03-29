using UnityEngine;
using UnityEngine.UI;



public class WindowButton : MonoBehaviour
{
    public WindowElement elements; // Current elements in this window
    public SilhouetteType silhouette; // Current silhouette

    [Header("Images")]
    public Image balconyImage;
    public Image curtainsImage;
    public Image silhouetteImage;

    [Header("Feedback")]
    public Image feedbackImage; // Visual feedback when selected

    private Puzzle2Manager manager;
    private Button button;

    public void Init(Puzzle2Manager puzzle2Manager)
    {
        manager = puzzle2Manager;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Setup(WindowElement e, SilhouetteType s)
    {
        // Assign new state
        elements = e;
        silhouette = s;

        SetClicked(false);
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        // Enable visuals based on active elements
        balconyImage.enabled = elements.HasFlag(WindowElement.Balcony);
        curtainsImage.enabled = elements.HasFlag(WindowElement.Curtains);
        silhouetteImage.enabled = elements.HasFlag(WindowElement.Silhouette);
    }

    void OnClick()
    {
        // Forward click to manager
        manager.OnWindowClicked(this);
    }

    public void SetClicked(bool value)
    {
        // Toggle selection feedback
        feedbackImage.enabled = value;
    }

    public bool IsCorrect(SilhouetteType target)
    {
        // Valid if all elements present + correct silhouette
        return elements.HasFlag(WindowElement.Balcony)
            && elements.HasFlag(WindowElement.Curtains)
            && elements.HasFlag(WindowElement.Silhouette)
            && silhouette == target;
    }
}