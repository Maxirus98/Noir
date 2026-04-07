using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneloader : MonoBehaviour
{
    public static sceneloader instance;
    public GameObject player;
    public Camera mainCamera;
    public Canvas ui;

    public Vector3 spawnPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(mainCamera.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadScene(string sceneName,Vector3 newSpawnPosition) 
    {
        spawnPosition = newSpawnPosition;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(sceneName);
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Move player
        player.transform.position = spawnPosition;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
