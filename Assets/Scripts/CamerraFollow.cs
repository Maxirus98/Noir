using UnityEngine;

public class CamerraFollow : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float followSpeed;

    public Vector3 positionCamera;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + positionCamera;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);

    }
}
