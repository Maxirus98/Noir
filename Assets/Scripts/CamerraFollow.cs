using UnityEngine;

public class CamerraFollow : MonoBehaviour
{
    private Transform target;
    private  Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float followSpeed;
    public Vector3 positionCamera;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(target.position.x, 0, -10f);
    }

    private void LateUpdate()
       {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x, target.position.y,
            transform.position.z) + positionCamera;

        if((desiredPosition - transform.position).sqrMagnitude > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}

   

