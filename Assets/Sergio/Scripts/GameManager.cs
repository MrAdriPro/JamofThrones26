using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject panel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            Time.timeScale = 0f; 
            panel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; 
            panel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        panel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    //void GameFinished()
    //{
    //    Time.timeScale = 0f; 
    //}

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        GameFinished();
    //    }
    //}
}