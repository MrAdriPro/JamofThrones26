using UnityEngine;
using UnityEngine.SceneManagement;

public class Playbutton : MonoBehaviour
{
   public void Play()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
