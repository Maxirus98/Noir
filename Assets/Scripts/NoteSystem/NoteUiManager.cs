using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Note Ui Manager to spawn and reference objects related to notes.
/// </summary>
public class NoteUiManager : MonoBehaviour
{
    public static NoteUiManager Instance;
    private static GameObject noteUi;
    private Animator noteAnimator;

    private void Awake()
    {
        if (Instance != null) { 
            Debug.Log("Can't have more instance for script" +  Instance.name);
        } else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        noteUi = transform.GetChild(0).gameObject;
        noteAnimator = GetComponentInChildren<Animator>(true);
    }

    public void CloseNote()
    {
        noteAnimator.CrossFade("Close", 0.1f);
    }

    public static void ToggleNoteUi()
    {
        noteUi.SetActive(!noteUi.activeInHierarchy);
    }

    public static void SetNoteData(IndiceData indiceData)
    {
        var texts = noteUi.GetComponentsInChildren<TextMeshProUGUI>(true);
        var description = texts[0].text = indiceData.Description;
        var extraInformation = texts[1].text = indiceData.ExtraInformation;
        var images = noteUi.GetComponentsInChildren<Image>(true);
        var background = images[0].color; 
        var clueImage = images[1];
        clueImage.sprite = indiceData.ItemSprite;
    }
}
