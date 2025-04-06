using UnityEngine;

public class Water : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStateManager.currentPlayerState = PlayerStateManager.PlayerState.Water;
            if (!collision.GetComponentInParent<FloppyFishController>().waterSound.isPlaying)
            {
                collision.GetComponentInParent<FloppyFishController>().waterSound.Play();
            }
            
            Debug.Log("Player is in water");
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStateManager.currentPlayerState = PlayerStateManager.PlayerState.Land;
            Debug.Log("Player is out of water");
        }
    }
}
