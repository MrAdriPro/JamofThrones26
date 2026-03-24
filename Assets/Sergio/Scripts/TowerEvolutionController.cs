using UnityEngine;

public class TowerEvolutionController : MonoBehaviour
{
    private GameObject[] activeTowers;

    void Start()
    {
        activeTowers = GameObject.FindGameObjectsWithTag("Tower");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpgradeModels();
        }
    }

    public void UpgradeModels()
    {
        foreach (GameObject tower in activeTowers)
        {
            TowerController towerController = tower.GetComponent<TowerController>();
            if (towerController != null)
            {
                towerController.Upgrade();
            }
        }
    }
}