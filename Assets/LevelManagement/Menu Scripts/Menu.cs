using DG.Tweening.Core.Easing;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    public virtual void ShowDisplay()
    {
        if (this == null || gameObject == null)
            return;

        gameObject.SetActive(true);
    }

    public virtual void HideDisplay()
    {
        if (this == null || gameObject == null)
            return;

        gameObject.SetActive(false);
    }

    public virtual void OnReturnToMainMenu()
    {
        AudioManager.Instance.StopMusic();

        // Détruire les Managers existant retournant main menu
        if (sceneloader.instance != null &&
            DupDestroy.instance != null &&
            DialogueManager.Instance != null &&
            NoteUiManager.Instance != null &&
            ProgressionManager.Instance != null)
        {
            Destroy(sceneloader.instance.gameObject);
            Destroy(DupDestroy.instance.gameObject);
            Destroy(DialogueManager.Instance.gameObject);
            Destroy(NoteUiManager.Instance.gameObject);
            Destroy(ProgressionManager.Instance.gameObject);
        }
    }


    public virtual void OnBack()
    {
        MenuManager.Instance.CloseMenu();
    }
}


