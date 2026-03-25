using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager shopInstance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI costText;

    public float actualCoins = 0f;
    public float upgradeCost = 100f;
    public float costMult = 2.5f;

    [SerializeField] PlayerModelController playerModel;
    [SerializeField] TowerEvolutionController towerModel;


    private void Awake()
    {
        if (shopInstance == null)
        {
            shopInstance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateText();
    }

    public void GetCoin(float coins)
    {
        actualCoins += coins;
        UpdateText();
    }

    void UpdateText()
    {
        coinText.text = "Coins: " + actualCoins.ToString("F0");
        costText.text = upgradeCost.ToString("F0");
    }

    public void BuyUpgrade()
    {
        if (!playerModel.CanEvolve)
        {
            costText.text = "MAX"; 
            return;
        }

        if (actualCoins >= upgradeCost)
        {
            playerModel.SwapModel();
            towerModel.UpgradeModels();

            actualCoins -= upgradeCost;
            upgradeCost *= costMult;

            UpdateText();

            if (!playerModel.CanEvolve)
            {
                costText.text = "MAX";
            }
        }
    }
}
