using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("Configs")]
    [SerializeField] private List<TransitionConfigSO> configs;

    private Dictionary<string, TransitionConfigSO> configMap;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build dictionary
        configMap = configs.ToDictionary(c => c.sceneName, c => c);
    }

    private TransitionConfigSO GetConfig(string sceneName)
    {
        if (configMap.TryGetValue(sceneName, out var config))
            return config;

        Debug.LogWarning($"No TransitionConfig found for scene: {sceneName}");
        return null;
    }

    public void TransitionToScene(string sceneName, FadeTransition transitionPrefab, float pauseDelay)
    {
        StartCoroutine(TransitionRoutine(sceneName, transitionPrefab, pauseDelay));
    }

    private IEnumerator TransitionRoutine(string sceneName, FadeTransition transitionPrefab, float pauseDelay)
    {
        InputManager.Instance.DisableAll();

        FadeTransition transition = Instantiate(transitionPrefab);

        var config = GetConfig(sceneName);
        if (config != null)
            transition.Init(config);

        yield return transition.PlayIn().WaitForCompletion();

        if (pauseDelay > 0f)
            yield return new WaitForSeconds(pauseDelay);

        yield return SceneManager.LoadSceneAsync(sceneName);

        yield return transition.PlayOut().WaitForCompletion();

        InputManager.Instance.EnableAll();
    }


    // Transition to fade in on the same scene when lose or win.
    public void FadeInCurrentScene(FadeTransition transitionPrefab, Menu menuToOpen, float pauseDelay)
    {
        StartCoroutine(FadeInCurrentSceneRoutine(transitionPrefab, menuToOpen, pauseDelay));
    }

    private IEnumerator FadeInCurrentSceneRoutine(FadeTransition transitionPrefab, Menu menuToOpen, float pauseDelay)
    {
        // 1. Block inputs
        InputManager.Instance.DisableAll();

        // 2. Spawn persistent transition
        FadeTransition transition = Instantiate(transitionPrefab);

        // 3. Fade IN
        yield return transition.PlayIn().WaitForCompletion();

        // 4. Pause
        if (pauseDelay > 0f)
            yield return new WaitForSeconds(pauseDelay);

        // 5. Open the correct menu 
        if (menuToOpen != null)
            MenuManager.Instance.OpenMenu(menuToOpen);

        // 6. Transition cleanup
        Destroy(transition.gameObject);

        // 7. Re-enable UI inputs
        InputManager.Instance.EnableAll();
    }
}