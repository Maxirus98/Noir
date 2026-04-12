using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeTransition : MonoBehaviour, ITransition
{
    private CanvasGroup canvasGroup;

    [SerializeField] private Image overlay;
    [SerializeField] private TMP_Text sceneText;

    private TransitionConfigSO config;

    public void Init(TransitionConfigSO newConfig)
    {
        config = newConfig;

        if (overlay != null)
            overlay.color = config.overlayColor;

        if (sceneText != null)
            sceneText.text = config.displayName;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!canvasGroup)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;
    }

    public Tween PlayIn()
    {
        return canvasGroup
            .DOFade(1f, config.duration)
            .SetEase(config.ease);
    }

    public Tween PlayOut()
    {
        return canvasGroup
            .DOFade(0f, config.duration)
            .SetEase(config.ease)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
                Destroy(gameObject);
            });
    }
}