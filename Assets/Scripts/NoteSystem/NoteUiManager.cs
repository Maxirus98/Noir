using System.Collections;
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

    private IndiceData lastIndiceData; // saves the latest dialogue from indice data

    [SerializeField]
    private GameObject newNoteBadge;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Can't have more instance for script" +  Instance.name);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        noteUi = transform.GetChild(0).gameObject;
        noteAnimator = GetComponentInChildren<Animator>(true);
    }

    private void OnEnable()
    {
        GameEvents.OnIndiceFound += HandleIndiceFound;
    }

    private void OnDisable()
    {
        GameEvents.OnIndiceFound -= HandleIndiceFound;
    }

    private void HandleIndiceFound(IndiceData data)
    {
        lastIndiceData = data; // stores latest indice data 
    }

    public void CloseNote()
    {
        noteAnimator.CrossFade("Close", 0.1f);
        newNoteBadge.SetActive(true);

        if (lastIndiceData != null)
            StartCoroutine(DelayedDialogue());
    }


    private IEnumerator DelayedDialogue()
    {
        yield return new WaitForSeconds(1f);

        if (lastIndiceData == null) yield break;
        if (lastIndiceData.DialogueOnFound == null) yield break;

        if (DialogueManager.Instance == null) yield break;
        if (DialogueManager.Instance.IsDialogueActive) yield break;
        
        // Start dialogue
        DialogueManager.Instance.StartDialogue(lastIndiceData.DialogueOnFound);

        lastIndiceData = null; // reset after indice use
       
        yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive);

        // 3 vols found ? event
        print("On3VolsFound");
        GameEvents.On3VolsFound?.Invoke();

    }


    public static void ToggleNoteUi()
    {
        noteUi.SetActive(!noteUi.activeInHierarchy);
    }

    /// <summary>
    /// Updates the note UI with the description, extra information, and item sprite from the specified indice data.
    /// </summary>
    /// <remarks>This method replaces the current contents of the note UI with the values from the provided
    /// indice data. The method assumes that the note UI contains the required text and image components in the expected
    /// order.</remarks>
    /// <param name="indiceData">The data object containing the description, extra information, and item sprite to display in the note UI. Cannot
    /// be null.</param>
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
