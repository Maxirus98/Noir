using UnityEngine;

public class DupDestroy : MonoBehaviour
{
    private static DupDestroy instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);  
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }
}
