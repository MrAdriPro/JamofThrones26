using UnityEngine;

public class ModelSwapper : MonoBehaviour
{
    public GameObject[] modelList;

    public float currentTek = 0;

    void Update()
    {
        if(currentTek != ShopManager.shopInstance.tekLevel) 
        {
            currentTek = ShopManager.shopInstance.tekLevel;

            UpdateModels();
        }
    }

    void UpdateModels() 
    {
        for (int i = 0; i < modelList.Length; i++)
        {
            if (modelList[i] != null && i != currentTek)
            {
                modelList[i].SetActive(false);
            }
            else modelList[i].SetActive(true);
        }
    }
}
