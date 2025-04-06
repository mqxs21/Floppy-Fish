using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillerRock : MonoBehaviour
{
    public GameObject partcielEffectDie;
    public CameraLocationTracker cameraLocationTracker;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(partcielEffectDie, transform.position, Quaternion.identity);
            cameraLocationTracker.ShakeCamera(1f,0.5f);
            StartCoroutine(WaitToDie());
        }
    }
    IEnumerator WaitToDie(){
        Time.timeScale = 0.9f;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
