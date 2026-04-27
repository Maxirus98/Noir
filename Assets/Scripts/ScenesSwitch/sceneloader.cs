using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class sceneloader : MonoBehaviour
{
    public static sceneloader instance;
    public GameObject player;
    public Canvas ui;

    public Vector3 spawnPosition;
    [SerializeField] private FadeTransition fadeTransition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(player);
        }
        else
        {
            Destroy(gameObject);
        }
        spawnPosition = new Vector3(4.3f, -3.96f, 0f);

    }

    public void LoadScene(string sceneName,Vector3 newSpawnPosition) 
    {
        TransitionManager.Instance.TransitionToScene(sceneName, fadeTransition, 1f);
        spawnPosition = newSpawnPosition;
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadSceneAsync(sceneName);
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Move player
        player.transform.position = spawnPosition;
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (scene.name == "RuePlaytestD")
        {
            AudioManager.Instance.CrossFadeMusic("Music_AmbianceRue");
        }
        else if (scene.name == "BarD") 
        {
           // AudioManager.Instance.StopMusic();
            AudioManager.Instance.CrossFadeMusic("Music_BarRadioStatic");
      
        }
        else if (scene.name == "BureauPlaytestD")
        {
           AudioManager.Instance.StopMusic();
        }
        else if (scene.name == "LaboratoireD")
        {
           // AudioManager.Instance.StopMusic();
            AudioManager.Instance.CrossFadeMusic("Music_TeleStatic");
        }
    }

}
