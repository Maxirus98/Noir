using UnityEngine;
using UnityEngine.UI;

public class WindowPuzzle : MonoBehaviour
{
    public TypeFenetre typefenetre;
    public TypeBalcon typeBalcon;
    public TypeSilhouette typeSilhouette;

    public Image debugImage;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnClickWindow);

        if (debugImage == null)
            debugImage = GetComponent<Image>();
    }

    void OnClickWindow()
    {
        PuzzleManager.Instance.SelectWindow(this);
    }

    public void SetSprite(Sprite sprite)
    {
        if (debugImage != null)
            debugImage.sprite = sprite;
    }
}