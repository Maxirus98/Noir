using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    private HashSet<string> flags = new HashSet<string>();

    void Awake()
    {
        Instance = this;
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
}