using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField] private DoorObstacle doorObstacle;
    [SerializeField] private Image healthbarImage;

    private void Update()
    {
        //healthbarImage.fillAmount = doorObstacle.currentHealth;
    }
}
