using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneloader : MonoBehaviour
{
    public static sceneloader instance;
    public GameObject player;
    public Canvas ui;

    public Vector3 spawnPosition = new Vector3(4.3f, -3.96f, 0f);
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
    }
}
