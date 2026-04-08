using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HiddenCameraManager : MonoBehaviour
{
    private Transform playerTransfom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransfom = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = -(playerTransfom.position - transform.position);
    }
}
