using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitForNextLevel());
        }
    }
    IEnumerator WaitForNextLevel()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
