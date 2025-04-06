using System.Collections;
using UnityEngine;

public class CameraLocationTracker : MonoBehaviour
{
    [Header("Player & Bounds")]
    public static bool overrideShake = false;
    public Transform player;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("Smooth Follow Settings")]
    [Tooltip("Time (in seconds) the camera takes to smooth from current to target position.")]
    public float smoothTime = 0.15f;

    [Header("Shake Settings (Called via ShakeCamera)")]
    private Camera cam;

    // Internal state
    private bool isShaking = false;
    private Vector3 velocity = Vector3.zero;  
    private float camHeight;
    private float camWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }
    void Update()
    {
//        Debug.Log(overrideShake);
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        if (!isShaking)
            StartCoroutine(ShakeCamEnum(duration, magnitude));
    }

    private IEnumerator ShakeCamEnum(float duration, float magnitude)
    {
        isShaking = true;
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration && !overrideShake)
        {
            float offsetX = Random.Range(-0.5f, 0.5f) * magnitude;
            float offsetY = Random.Range(-0.5f, 0.5f) * magnitude;
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }

    void LateUpdate()
    {
        if (overrideShake && isShaking)
        {
            StopAllCoroutines();
            isShaking = false;
            overrideShake = false;
        }

        if (player != null && !isShaking)
        {
            float clampedX = Mathf.Clamp(player.position.x, minX + camWidth, maxX - camWidth);
            float clampedY = Mathf.Clamp(player.position.y, minY + camHeight, maxY - camHeight);
            Vector3 targetPos = new Vector3(clampedX, clampedY, transform.position.z);

            transform.position = Vector3.SmoothDamp(
                transform.position, 
                targetPos, 
                ref velocity, 
                smoothTime
            );
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(maxX, maxY, 0));
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY, 0));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0));
    }
}