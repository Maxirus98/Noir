using Unity.VisualScripting;
using UnityEngine;

public class Indice : MonoBehaviour
{
    [field: SerializeField]
    public IndiceData data { get; private set; }
    [field: SerializeField]
    public float clueFoundScale { get; set; }
}
