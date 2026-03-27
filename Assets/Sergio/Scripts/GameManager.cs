using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject panel;

    void Update()
    {
        PauseGame();
    }

    void PauseGame()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            Time.timeScale = gameIsPaused ? 1 : 0;
            if (gameIsPaused)
            {
                panel.SetActive(true);
            }
            else panel.SetActive(false);
        }
    }

    void GameFinished()
    {
        // Acciones al terminar el juego
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            GameFinished();
        }
    }
    public void ResumeGame() 
    { 
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
