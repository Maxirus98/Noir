using System.Collections;
using UnityEngine;

public class VillainGoesAway : MonoBehaviour
{
    [SerializeField] private float targetY = 20f;
    [SerializeField] private float speed = 5f;

    [SerializeField] private string doneFlag = "Villain_DONE";

    private bool isMoving = false;

    private void Start()
    {
        // Si déjà fait -> disparaît direct
        if (ProgressionManager.Instance.HasFlag(doneFlag))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnMidVillainDone += MoveUp;
    }

    private void OnDisable()
    {
        GameEvents.OnMidVillainDone -= MoveUp;
    }

    private void MoveUp()
    {
        if (isMoving) return;
        print("up");
        isMoving = true;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float duration = 3f; // temps total du mouvement
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            t = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        // Assure position finale parfaite
        transform.position = targetPos;

        gameObject.SetActive(false);
    }
}