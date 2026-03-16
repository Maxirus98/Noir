using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public List<WindowPuzzle> allWindows;

    int currentStepIndex = 0;

    [Header("Debug Sprites")]
    [SerializeField] Sprite balconSprite;
    [SerializeField] Sprite rideauSprite;
    [SerializeField] Sprite manteauSprite;

    [Header("Empty Window Sprites")]
    [SerializeField] List<Sprite> emptySprites;

    [Header("Puzzle Zones")]
    [SerializeField] int balconMinIndex = 0;
    [SerializeField] int balconMaxIndex = 3;

    [SerializeField] int otherMinIndex = 4;
    [SerializeField] int otherMaxIndex = 15;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnPuzzleButtons();
    }

    public void SelectWindow(WindowPuzzle window)
    {
        if (VerifyWindow(window))
        {
            currentStepIndex++;

            SpawnPuzzleButtons();

            Debug.Log("Bonne réponse : " + currentStepIndex + "/3");

            if (currentStepIndex >= 3)
                PuzzleSuccess();
        }
        else
        {
            ResetPuzzle();
        }
    }

    bool VerifyWindow(WindowPuzzle window)
    {
        if (currentStepIndex == 0)
        {
            return window.typeBalcon == TypeBalcon.Balcon &&
                   window.typefenetre == TypeFenetre.Aucun &&
                   window.typeSilhouette == TypeSilhouette.Aucun;
        }

        if (currentStepIndex == 1)
        {
            return window.typefenetre == TypeFenetre.Rideaux &&
                   window.typeBalcon == TypeBalcon.Aucun &&
                   window.typeSilhouette == TypeSilhouette.Aucun;
        }

        if (currentStepIndex == 2)
        {
            return window.typeSilhouette == TypeSilhouette.LongManteau &&
                   window.typeBalcon == TypeBalcon.Aucun &&
                   window.typefenetre == TypeFenetre.Aucun;
        }

        return false;
    }

    void SpawnPuzzleButtons()
    {
        foreach (WindowPuzzle window in allWindows)
        {
            window.typeBalcon = TypeBalcon.Aucun;
            window.typefenetre = TypeFenetre.Aucun;
            window.typeSilhouette = TypeSilhouette.Aucun;

            window.SetSprite(GetRandomEmptySprite());
        }

        int balconIndex = Random.Range(balconMinIndex, balconMaxIndex + 1);

        int rideauIndex;
        int manteauIndex;

        while (true)
        {
            rideauIndex = Random.Range(otherMinIndex, otherMaxIndex + 1);
            manteauIndex = Random.Range(otherMinIndex, otherMaxIndex + 1);

            if (rideauIndex != manteauIndex)
                break;
        }

        // BALCON
        WindowPuzzle balconWindow = allWindows[balconIndex];
        balconWindow.typeBalcon = TypeBalcon.Balcon;
        balconWindow.SetSprite(balconSprite);

        // RIDEAU
        WindowPuzzle rideauWindow = allWindows[rideauIndex];
        rideauWindow.typefenetre = TypeFenetre.Rideaux;
        rideauWindow.SetSprite(rideauSprite);

        // MANTEAU
        WindowPuzzle manteauWindow = allWindows[manteauIndex];
        manteauWindow.typeSilhouette = TypeSilhouette.LongManteau;
        manteauWindow.SetSprite(manteauSprite);

        Debug.Log($"Spawn : Balcon:{balconIndex} Rideau:{rideauIndex} Manteau:{manteauIndex}");
    }

    Sprite GetRandomEmptySprite()
    {
        if (emptySprites == null || emptySprites.Count == 0)
            return null;

        return emptySprites[Random.Range(0, emptySprites.Count)];
    }

    void ResetPuzzle()
    {
        Debug.Log("Reset puzzle");

        currentStepIndex = 0;
        SpawnPuzzleButtons();
    }

    void PuzzleSuccess()
    {
        Debug.Log("Puzzle réussi !");
        gameObject.SetActive(false);
        FindAnyObjectByType<NoirMouvement>().GetComponent<PlayerInput>().enabled = true;

    }
}