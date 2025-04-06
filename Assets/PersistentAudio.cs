using UnityEngine;

public class PersistentAudio : MonoBehaviour
{
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
}
