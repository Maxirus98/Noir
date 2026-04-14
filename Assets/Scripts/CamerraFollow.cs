using UnityEngine;

public class CamerraFollow : MonoBehaviour
{
    private Transform target;
    private  Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float followSpeed;
    public Vector3 positionCamera;

    [SerializeField]
    private Transform cameraTransformMinX;
    [SerializeField]
    private Transform cameraTransformMaxX;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(target.position.x, 0, -10f);
    }

    private void LateUpdate()
       {
        if (target == null) return;
        var targetClampX = Mathf.Clamp(target.position.x, cameraTransformMinX.position.x + Camera.main.orthographicSize * 2, cameraTransformMaxX.position.x - Camera.main.orthographicSize * 2);

        Vector3 desiredPosition = new Vector3(
            targetClampX, target.position.y,
            transform.position.z) + positionCamera;

        if((desiredPosition - transform.position).sqrMagnitude > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}

   

