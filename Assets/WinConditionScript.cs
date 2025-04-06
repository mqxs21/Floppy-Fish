using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionScript : MonoBehaviour
{
    public GameObject partcielEffectWin;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(partcielEffectWin, transform.position, Quaternion.identity);
            StartCoroutine(WaitForNextLevel());
        }
    }
    IEnumerator WaitForNextLevel()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
