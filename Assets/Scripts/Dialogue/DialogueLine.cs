using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueLine
{
    public string speaker;
    public Sprite sprite;
    [TextArea(0,3)] public string text;
    public float typingSpeed;
}