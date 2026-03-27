using UnityEngine;
using UnityEngine.SceneManagement;

public class Playbutton : MonoBehaviour
{
    public string levelName = "LevelScene";
   public void Play()
    {
        SceneManager.LoadScene(levelName);
    }
}
