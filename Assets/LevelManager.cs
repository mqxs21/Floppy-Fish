using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void NextScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
