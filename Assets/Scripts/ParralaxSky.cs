using UnityEngine;

public class ParallaxSky : MonoBehaviour
{
    [Header("Reference")]
    public Transform player;
    public Camera cam;

    [Header("Parallax Settings")]
    public float parallaxFactor = 0.5f; // 0 = static, 1 = full follow
    public float moveThreshold = 0.01f;  // Minimum movement required to update

    private float startX;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        startX = transform.position.x;
    }

    void LateUpdate()
    {
        float offsetX = player.position.x - cam.transform.position.x;

            float parallaxX = startX + offsetX * parallaxFactor;
            transform.position = Vector3.Lerp(transform.position, new Vector3(parallaxX, transform.position.y, transform.position.z), Time.deltaTime * 5f);

        
    }
}
