using TMPro;
using UnityEngine;

public class OnHoverNote : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        
    }

    // On Enter
    public void SetUnderline()
    {
        textMeshProUGUI.fontStyle = FontStyles.Underline;
    }

    //On Exit
    public void SetNormalFont()
    {
        textMeshProUGUI.fontStyle = FontStyles.Normal;
    }
}
