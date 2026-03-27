using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField] private DoorObstacle doorObstacle;
    [SerializeField] private Image healthbarImage;
    [SerializeField] private Image shieldBarImage;

    private void Update()
    {
        healthbarImage.fillAmount = doorObstacle.currentHealth / doorObstacle.maxHealth;
        if (shieldBarImage != null)
        {
            shieldBarImage.fillAmount = doorObstacle._currentEscudo / 100f;
            shieldBarImage.gameObject.SetActive(doorObstacle._currentEscudo > 0);
        }
       
    }
}
