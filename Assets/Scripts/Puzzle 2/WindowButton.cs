using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public WindowElement elements;
    public SilhouetteType silhouette;

    [Header("Images")]
    public Image windowImage;
    public Image balconyImage;
    public Image curtainsImage;
    public Image silhouetteImage;

    [Header("Feedback")]
    [SerializeField] private GameObject feedbackObject; // parent object (enable/disable)
    private Image feedbackImage; // image inside

    [Header("Colors")]
    [SerializeField] private Color hoverColor = new Color(1, 1, 1, 0.2f);
    [SerializeField] private Color clickColor = Color.green;

    private Puzzle2Manager manager;
    private Button button;

    private bool isClicked = false;

    public void Init(Puzzle2Manager puzzle2Manager)
    {
        manager = puzzle2Manager;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        feedbackImage = feedbackObject.GetComponent<Image>();
        feedbackObject.SetActive(false);
    }

    public void Setup(WindowElement e, SilhouetteType s)
    {
        elements = e;
        silhouette = s;

        isClicked = false;
        feedbackObject.SetActive(false);

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        windowImage.enabled = true;

        balconyImage.enabled = elements.HasFlag(WindowElement.Balcony);
        curtainsImage.enabled = elements.HasFlag(WindowElement.Curtains);
        silhouetteImage.enabled = elements.HasFlag(WindowElement.Silhouette);
    }

    void OnClick()
    {
        manager.OnWindowClicked(this);
    }

    public void SetClicked(bool value)
    {
        isClicked = value;

        if (value)
        {
            feedbackObject.SetActive(true);
            feedbackImage.color = clickColor;
        }
    }

    // Hover enter
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isClicked) return;

        feedbackObject.SetActive(true);
        feedbackImage.color = hoverColor;
    }

    // Hover exit
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isClicked) return;

        feedbackObject.SetActive(false);
    }

    public bool IsCorrect(SilhouetteType target)
    {
        return elements.HasFlag(WindowElement.Balcony)
            && elements.HasFlag(WindowElement.Curtains)
            && elements.HasFlag(WindowElement.Silhouette)
            && silhouette == target;
    }
}