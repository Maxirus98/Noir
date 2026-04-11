using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    [SerializeField] private HashSet<string> flags = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetFlag(string flag)
    {
        flags.Add(flag);
        Debug.Log("Flag added: " + flag);
    }

    public bool HasFlag(string flag)
    {
        return flags.Contains(flag);
    }


    public bool HasAllFlags(List<string> requiredFlags)
    {
        if (requiredFlags == null || requiredFlags.Count == 0)
            return true;

        foreach (string flag in requiredFlags)
        {
            if (!HasFlag(flag))
            {
                Debug.Log("Missing flag: " + flag);
                return false;
            }
        }

        return true;
    }






}