using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Noir/Dialogue")]
public class DialogueData : ScriptableObject
{
    public List<string> requiredFlags = new List<string>();
    
    public List<string> setFlags = new List<string>();
    
    [field: SerializeField]
    public IndiceData indiceData { get; private set; }

    public List<DialogueLine> lines = new List<DialogueLine>();

}

