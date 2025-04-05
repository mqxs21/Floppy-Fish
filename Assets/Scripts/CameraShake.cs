using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Camera cam;
    private float camHeight;
    private float camWidth;
    private bool isShaking = false;
    public static bool overrideShake = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        camHeight = cam.orthographicSize;
        camWidth = cam.orthographicSize * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
