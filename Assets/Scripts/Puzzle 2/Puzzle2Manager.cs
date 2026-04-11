using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Puzzle2Manager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private List<WindowButton> windows; // All puzzle windows

    [Header("Sprites")]
    [SerializeField] private Sprite windowSprite;
    [SerializeField] private Sprite balconySprite;
    [SerializeField] private Sprite curtainsSprite;

    [SerializeField] private Sprite longCoatSprite;
    [SerializeField] private Sprite thinSprite;
    [SerializeField] private Sprite tallSprite;
    [SerializeField] private Sprite smallSprite;

    [Header("Gameplay")]
    [SerializeField] private SilhouetteType target = SilhouetteType.LongCoat; // Target silhouette to find
    [SerializeField] private int minCorrectStart = 2; // Base number of valid windows
    [SerializeField] private int maxStages = 3; // Total stages to complete

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI stageText; // Displays current stage
    [SerializeField] private TextMeshProUGUI dialogueText; // Displays feedback messages

    [Header("Audio")]
    [SerializeField] private string successStepSound;
    [SerializeField] private string failSound;
    [SerializeField] private string completeSound;

    private List<WindowButton> correct = new List<WindowButton>(); // All valid windows for current round
    private List<WindowButton> selected = new List<WindowButton>(); // Player selections

    private int stage = 0;

    void Start()
    {
        // Initialize all windows with manager reference
        foreach (var window in windows)
            window.Init(this);

        UpdateStageUI();
        Shuffle();
    }

    void Shuffle()
    {
        // Reset state
        correct.Clear();
        selected.Clear();

        int minCorrect = minCorrectStart + stage; // Increase difficulty per stage

        // Randomize all windows
        foreach (var window in windows)
        {
            WindowElement e = GetRandomElements();
            SilhouetteType s = GetRandomSilhouette(e);

            window.Setup(e, s);
            ApplySprites(window);
        }

        // Force a minimum number of valid windows
        List<WindowButton> shuffled = new List<WindowButton>(windows);
        ShuffleList(shuffled);

        for (int i = 0; i < minCorrect && i < shuffled.Count; i++)
        {
            var window = shuffled[i];

            window.Setup(
                WindowElement.Balcony | WindowElement.Curtains | WindowElement.Silhouette,
                target
            );

            ApplySprites(window);
        }

        // Cache valid windows for validation
        foreach (var window in windows)
        {
            if (window.IsCorrect(target))
                correct.Add(window);
        }
    }

    void ApplySprites(WindowButton window)
    {
        // Always apply window background
        if (window.windowImage != null)
            window.windowImage.sprite = windowSprite;

        // Apply correct sprite based on active elements
        if (window.balconyImage.enabled)
            window.balconyImage.sprite = balconySprite;

        if (window.curtainsImage.enabled)
            window.curtainsImage.sprite = curtainsSprite;

        if (window.silhouetteImage.enabled)
            window.silhouetteImage.sprite = GetSilhouetteSprite(window.silhouette);
    }

    Sprite GetSilhouetteSprite(SilhouetteType type)
    {
        // Return sprite matching silhouette type
        switch (type)
        {
            case SilhouetteType.LongCoat: return longCoatSprite;
            case SilhouetteType.Thin: return thinSprite;
            case SilhouetteType.Tall: return tallSprite;
            case SilhouetteType.Small: return smallSprite;
        }

        return null;
    }

    WindowElement GetRandomElements()
    {
        // Random combination of elements using flags
        WindowElement windowElement = WindowElement.None;

        if (Random.value > 0.5f) windowElement |= WindowElement.Balcony;
        if (Random.value > 0.5f) windowElement |= WindowElement.Curtains;
        if (Random.value > 0.5f) windowElement |= WindowElement.Silhouette;

        return windowElement;
    }

    SilhouetteType GetRandomSilhouette(WindowElement windowElement)
    {
        // Only assign silhouette if window has one
        if (!windowElement.HasFlag(WindowElement.Silhouette))
            return SilhouetteType.None;

        // Increase variety as stages progress
        int maxType = 2 + stage;
        maxType = Mathf.Clamp(maxType, 2, 4);

        return (SilhouetteType)Random.Range(1, maxType + 1);
    }

    public void OnWindowClicked(WindowButton window)
    {
        // Prevent double selection
        if (selected.Contains(window))
            return;

        selected.Add(window);
        window.SetClicked(true);

        // Wrong selection -> reset puzzle
        if (!window.IsCorrect(target))
        {
            AudioManager.Instance.PlaySound(failSound);
            stage = 0;
            UpdateStageUI();
            ShowDialogue("N'oublie pas de suivre l'ordre en trouvant 3 éléments");
            Shuffle();
            return;
        }

        // All correct windows found -> next stage
        if (selected.Count == correct.Count)
        {
            NextStage();
        }
    }

    void NextStage()
    {
        stage++;

        ShowDialogue("Parfait continuons !");

        // Puzzle complete
        if (stage >= maxStages)
        {
            AudioManager.Instance.PlaySound(completeSound);
            ShowDialogue("Te voici fugitif !");
            gameObject.SetActive(false); 
            GameEvents.OnPuzzle2Completed?.Invoke("Puzzle2"); // subscrition in PuzzleInteraction
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
        // Update stage text
        if (stageText != null)
            stageText.text = "Stage " + (stage) + " / " + maxStages;
    }

    void ShowDialogue(string message)
    {
        // Display feedback message
        if (dialogueText != null)
            dialogueText.text = message;
    }

    // Sort Algo
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}