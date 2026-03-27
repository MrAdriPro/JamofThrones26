using UnityEngine;

public class SingleModerAvtivate : MonoBehaviour
{
    [SerializeField] GameObject[] models;
    public int ageNum = 0;
    void Update()
    {
        if (ageNum != ShopManager.shopInstance.tekLevel)
        {
            ModelSwapper(false);
        }
        else ModelSwapper(true);
    }

    void ModelSwapper(bool activate)
    {
        if (activate)
        {
            foreach (var model in models)
            {
                model.SetActive(true);
            }
        }
        else
        {
            foreach (var model in models)
            {
                model.SetActive(false);
            }
        }
    }
}
