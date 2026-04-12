using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Transitions/Config")]
public class TransitionConfigSO : ScriptableObject
{
    public string sceneName;
    public string displayName;
    
    public float duration = 0.5f;
    
    public Ease ease = Ease.InOutCubic;
    public Color overlayColor = Color.black;
}