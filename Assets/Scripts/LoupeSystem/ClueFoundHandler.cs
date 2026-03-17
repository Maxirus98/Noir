using UnityEngine;


/// <summary>
/// Script sur le ClueFound prefab qui fait disparaitre la loupe lorsque l'indice est trouvé et se charge de faire apparaitre un effet visuel, joue un son et se détruit automatiquement
/// </summary>
public class ClueFoundHandler : MonoBehaviour
{
    public IndiceData indiceData { get; set; }

    private InspectionManager inspectionManager;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        inspectionManager = FindAnyObjectByType<InspectionManager>();
        inspectionManager.ToggleLoupe();

        // TODO: Play sound of clue found
        // Détruire cette objet après son animation de fadeout
        var fadeOutAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        Destroy(gameObject, fadeOutAnimation.length);
    }

    private void OnDestroy()
    {
        NoteUiManager.SetData(indiceData);
        NoteUiManager.ToggleNoteUi();
    }
}
