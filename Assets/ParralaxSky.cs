using UnityEngine;

public class ParalaxSky : MonoBehaviour
{
    [Header("Reference")]
    public Transform player; // assign in inspector
    public Camera cam;

    [Header("Parallax Settings")]
    public float parallaxFactor = 0.5f; // 0 = static, 1 = moves with player
    private float startX;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        startX = transform.position.x;
    }

    void LateUpdate()
    {
        // Get how far the player is from the camera center in world units
        float playerOffsetX = player.position.x - cam.transform.position.x;

        // Apply a parallax shift based on that offset
        float parallaxX = startX + playerOffsetX * parallaxFactor;

        // Move sky to match (Y/Z stay the same)
        transform.position = new Vector3(parallaxX, transform.position.y, transform.position.z);
    }
}
