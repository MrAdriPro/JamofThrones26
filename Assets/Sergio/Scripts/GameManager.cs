using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameIsPaused;

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
}
