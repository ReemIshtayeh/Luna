using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneControler : MonoBehaviour
{  
    public static SceneControler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void NextLevel()
    {
        //  AudioManager.Instance.PlayWinSFX(); // 🔊 Play win sound
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
