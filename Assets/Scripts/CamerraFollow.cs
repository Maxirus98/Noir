using UnityEngine;

public class CamerraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float followSpeed;
    public Vector3 positionCamera;

    [SerializeField] private Transform cameraTransformMinX;
    [SerializeField] private Transform cameraTransformMaxX;

    [Header("Cinematic Cam Puzzle 2")]
    public bool isInCinematic = false;
    private Vector3 cinematicTarget;
    public float cinematicSpeed = 3f;



    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player avec le tag -Player- introuvable.");
            return;
        }

        target = player.transform;

        // Snap immediat apres scene loaded
        transform.position = GetDesiredPosition();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Cam�ra cin�matique
        if (isInCinematic)
        {
            transform.position = Vector3.Lerp(transform.position, cinematicTarget, cinematicSpeed * Time.deltaTime);
            return;
        }

        Vector3 desiredPosition = GetDesiredPosition();

        if ((desiredPosition - transform.position).sqrMagnitude > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }

    // 2D orthographic
    // moiti� de la hauteur = orthographicSize  
    // moiti� de la largeur = orthographicSize * aspect
    private Vector3 GetDesiredPosition()
    {
        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        float targetClampX = Mathf.Clamp(
            target.position.x,
            cameraTransformMinX.position.x + camHalfWidth,
            cameraTransformMaxX.position.x - camHalfWidth
        );

        return new Vector3(targetClampX, target.position.y, transform.position.z) + positionCamera;
    }

    public void MoveToCinematic(Vector3 targetPos)
    {
        isInCinematic = true;
        cinematicTarget = targetPos;
    }

    public void StopCinematic()
    {
        isInCinematic = false;
    }
}