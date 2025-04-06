using UnityEngine;

public class PersistentAudio : MonoBehaviour
{
    public AudioSource backgroundMusic;
    void Awake()
    {
        // If another copy exists, destroy this one (prevents duplicates on reload)
        if (FindObjectsByType<PersistentAudio>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Water)
        {
            backgroundMusic.pitch = 0.8f;
        }
        else if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Land)
        {
            backgroundMusic.pitch = 1.0f;
        }
    }
}
