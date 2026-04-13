using UnityEngine;
using System.Collections.Generic;
using TMPro;

// Create matched pair silhouette
[System.Serializable]
public class SilhouetteSpritePair
{
    public SilhouetteType type; // Silhouette enum key
    public Sprite sprite; // Associated sprite
}

public class Puzzle2Manager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private List<WindowButton> windows; // All puzzle window buttons

    [Header("Sprites")]
    [SerializeField] private Sprite windowSprite; // Base window background
    [SerializeField] private Sprite balconySprite; // Balcony visual
    [SerializeField] private Sprite curtainsSprite; // Curtains visual

    [Header("Silhouettes")]
    [SerializeField] private List<SilhouetteSpritePair> silhouetteSprites; // Mapping type -> sprite

    private Dictionary<SilhouetteType, Sprite> spriteMap; 

    [Header("Gameplay")]
    [SerializeField] private SilhouetteType target = SilhouetteType.LongCoat; // Correct silhouette
    [SerializeField] private int maxStages = 3; // Total puzzle stages

    [Header("Distribution")]
    [SerializeField] private List<int> correctPerStage = new List<int>() { 3, 2, 1 }; // Correct count per stage
    [SerializeField] private int fakeLongCoatCount = 2; // Fake target windows count
    [SerializeField] private int noSilhouetteCount = 3; // Empty silhouette windows count
    // The rest is random silhouettes

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI stageText; // Stage display text
    [SerializeField] private TextMeshProUGUI dialogueText; // Feedback dialogue

    [Header("Audio")]
    [SerializeField] private string successStepSound; 
    [SerializeField] private string failSound; 
    [SerializeField] private string completeSound; 

    private List<WindowButton> correct = new List<WindowButton>(); // Valid windows cache
    private List<WindowButton> selected = new List<WindowButton>(); // Player selections

    private int stage = 0;

    void Start()
    {
        // Build sprite dictionary 
        spriteMap = new Dictionary<SilhouetteType, Sprite>();
        foreach (var pair in silhouetteSprites)
            spriteMap[pair.type] = pair.sprite;

        // Init all windows 
        foreach (var window in windows)
            window.Init(this);

        UpdateStageUI();
        Shuffle();
    }

    void Shuffle()
    {
        // Reset round state
        correct.Clear();
        selected.Clear();

        int correctCount = GetCorrectCountForStage();

        // Shuffle window order for distribution
        List<WindowButton> shuffled = new List<WindowButton>(windows);
        ShuffleList(shuffled);

        int index = 0;

        // 1. Correct windows (full kit + target slhouette)
        for (int i = 0; i < correctCount && index < shuffled.Count; i++, index++)
        {
            var w = shuffled[index];

            w.Setup(
                WindowElement.Balcony | WindowElement.Curtains | WindowElement.Silhouette,
                target
            );

            ApplySprites(w);
        }

        // 2. Incorrect fake target (missing at least one element)
        for (int i = 0; i < fakeLongCoatCount && index < shuffled.Count; i++, index++)
        {
            var w = shuffled[index];

            WindowElement e = GetRandomElements() | WindowElement.Silhouette;

            // Remove one element to invalidate
            if (e.HasFlag(WindowElement.Balcony) && e.HasFlag(WindowElement.Curtains))
            {
                if (Random.value > 0.5f)
                    e &= ~WindowElement.Balcony;
                else
                    e &= ~WindowElement.Curtains;
            }

            w.Setup(e, target);
            ApplySprites(w);
        }

        // 3. Windows with no silhouette
        for (int i = 0; i < noSilhouetteCount && index < shuffled.Count; i++, index++)
        {
            var w = shuffled[index];

            WindowElement e = GetRandomElements();
            e &= ~WindowElement.Silhouette;

            w.Setup(e, SilhouetteType.None);
            ApplySprites(w);
        }

        // 4. All the rest of windows -> random non-target silhouettes
        for (; index < shuffled.Count; index++)
        {
            var w = shuffled[index];

            WindowElement e = GetRandomElements() | WindowElement.Silhouette;
            SilhouetteType s = GetRandomNonTargetSilhouette();

            w.Setup(e, s);
            ApplySprites(w);
        }

        // Cache valid windows for win condition
        foreach (var window in windows)
        {
            if (window.IsCorrect(target))
                correct.Add(window);
        }
    }

    int GetCorrectCountForStage()
    {
        // Get correct count safely per stage
        if (correctPerStage == null || correctPerStage.Count == 0)
            return 1;

        int index = Mathf.Clamp(stage, 0, correctPerStage.Count - 1);
        return correctPerStage[index];
    }

    void ApplySprites(WindowButton window)
    {
        // Apply base window background
        if (window.windowImage != null)
            window.windowImage.sprite = windowSprite;

        // Apply element visuals
        if (window.balconyImage.enabled)
            window.balconyImage.sprite = balconySprite;

        if (window.curtainsImage.enabled)
            window.curtainsImage.sprite = curtainsSprite;

        // Apply silhouette sprite
        if (window.silhouetteImage.enabled)
            window.silhouetteImage.sprite = GetSilhouetteSprite(window.silhouette);
    }

    Sprite GetSilhouetteSprite(SilhouetteType type)
    {
        // Get sprite from dictionary safely
        if (spriteMap.TryGetValue(type, out var sprite))
            return sprite;

        return null;
    }

    SilhouetteType GetRandomNonTargetSilhouette()
    {
        // Get all non-target silhouettes dynamically
        List<SilhouetteType> types = new List<SilhouetteType>();

        foreach (SilhouetteType type in System.Enum.GetValues(typeof(SilhouetteType)))
        {
            if (type != SilhouetteType.None && type != target)
                types.Add(type);
        }

        return types[Random.Range(0, types.Count)];
    }

    WindowElement GetRandomElements()
    {
        // Random combination using bit flags
        WindowElement e = WindowElement.None;

        if (Random.value > 0.5f) e |= WindowElement.Balcony;
        if (Random.value > 0.5f) e |= WindowElement.Curtains;
        if (Random.value > 0.5f) e |= WindowElement.Silhouette;

        return e;
    }

    public void OnWindowClicked(WindowButton window)
    {
        // Prevent duplicate clicks
        if (selected.Contains(window))
            return;

        selected.Add(window);
        window.SetClicked(true);

        // Wrong selection resets puzzle
        if (!window.IsCorrect(target))
        {
            AudioManager.Instance.PlaySound(failSound);
            stage = 0;
            UpdateStageUI();
            ShowDialogue("N'oublie pas de suivre l'ordre en trouvant 3 éléments");
            Shuffle();
            return;
        }

        // All correct found -> next stage
        if (selected.Count == correct.Count)
        {
            NextStage();
        }
    }

    void NextStage()
    {
        stage++;

        ShowDialogue("Parfait continuons !");

        // Puzzle completion condition
        if (stage >= maxStages)
        {
            AudioManager.Instance.PlaySound(completeSound);
            ShowDialogue("Te voici fugitif !");
            gameObject.SetActive(false);
            GameEvents.OnPuzzle2Completed?.Invoke("Puzzle2");
            return;
        }
        else
        {
            AudioManager.Instance.PlaySound(successStepSound);
        }

        UpdateStageUI();
        Shuffle();
    }

    void UpdateStageUI()
    {
        // Update stage counter UI
        if (stageText != null)
            stageText.text = "Stage " + (stage) + " / " + maxStages;
    }

    void ShowDialogue(string message)
    {
        // Display feedback message
        if (dialogueText != null)
            dialogueText.text = message;
    }

    void ShuffleList<T>(List<T> list)
    {
        // Fisher-Yates shuffle algorithm
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
